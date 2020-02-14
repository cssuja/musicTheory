using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.Extensions.Options;
using MusicTheory.Configuration;
using MusicTheory.Features.LessonFeature.Models;
using MusicTheory.Features.LessonFeature.OptionFeature;

namespace MusicTheory.Features.LessonFeature
{
    public interface ILessonService
    {
        Lesson GetLesson(int id, int maxNumberOfQuestions);
        List<Lesson> GetLessons();
        int MergeLesson(Lesson lesson);
        int MergeQuestion(Question question, int lessonId);
        int MergeOption(QuestionOption option, int questionId);
        void DeleteOption(int questionId, int optionId, OptionType typeId);
        void DeleteQuestion(int lessonId, int questionId);
        void DeleteLesson(int lessonId);
        List<SelectItem> GetExistingOptions();
    }
    public class LessonService : ILessonService
    {
        private readonly ILessonRepository _repository;
        private readonly MusicTheoryConfiguration _config;
        private readonly IOptionRepositoryFactory _optionRepositoryFactory;

        public LessonService(ILessonRepository repository, IOptions<MusicTheoryConfiguration> config, IOptionRepositoryFactory optionRepositoryFactory )
        {
            _repository = repository;
            _config = config.Value;
            _optionRepositoryFactory = optionRepositoryFactory;
        }
        public Lesson GetLesson(int id, int maxNumberOfQuestions)
        {
            var maxQuestions = maxNumberOfQuestions > 0 ? maxNumberOfQuestions : _config.AppSettings.DefaultNumberOfQuestions;
            var random = new Random();
            Lesson lesson;

            using (var cnn = new SqlConnection(_config.ConnectionStrings.MusicTheoryConnectionString))
            {
                cnn.Open();

                using (var t = cnn.BeginTransaction())
                {
                    lesson = _repository.GetLesson(id, cnn, t);

                    var questions = _repository.GetQuestionsForLesson(id, maxQuestions, cnn, t);

                    foreach (var question in questions)
                    {
                        if(question.Options == null)
                        {
                            question.Options = new List<QuestionOption>();
                        }

                        var skeletonOptions = _repository.GetSkeletonOptions(cnn, t, question.Id);
                        
                        foreach(var option in skeletonOptions)
                        {
                            var optionObject = _optionRepositoryFactory.CreateRepository(option.TypeId).GetOption(cnn, t, option.Id);
                            option.Option = optionObject;
                            question.Options.Add(option);
                        }
                    }

                    lesson.Questions = questions;
                }
            }

            lesson.Questions= lesson.Questions.OrderBy(q => random.Next()).ToList();
            foreach(var question in lesson.Questions)
            {
                question.Options = question.Options.OrderBy(q => random.Next()).ToList();
            }
            return lesson ;
        }

        public List<Lesson> GetLessons()
        {
            List<Lesson> lessons;
            using (var cnn = new SqlConnection(_config.ConnectionStrings.MusicTheoryConnectionString))
            {
                cnn.Open();

                using (var t = cnn.BeginTransaction())
                {
                    lessons = _repository.GetLessons(cnn, t);
                }
            }
            return lessons;
        }

        public int MergeOption(QuestionOption option, int questionId)
        {
            using (var cnn = new SqlConnection(_config.ConnectionStrings.MusicTheoryConnectionString))
            {
                cnn.Open();

                using (var t = cnn.BeginTransaction())
                {
                    option.Id = _optionRepositoryFactory.CreateRepository(option.TypeId).MergeOption(cnn, t, option);
                    _repository.MergeQuestionOption(cnn, t, questionId, option);
                    t.Commit();
                }
            }

            return option.Id;
        }

        public int MergeQuestion(Question question, int lessonId)
        {
            using (var cnn = new SqlConnection(_config.ConnectionStrings.MusicTheoryConnectionString))
            {
                cnn.Open();

                using (var t = cnn.BeginTransaction())
                {
                    question.Id = _repository.MergeQuestion(cnn, t, question);
                    _repository.MergeLessonQuestion(lessonId, cnn, t, question.Id);
                    t.Commit();
                }
            }
            return question.Id;
        }

        public int MergeLesson(Lesson lesson)
        {
            using (var cnn = new SqlConnection(_config.ConnectionStrings.MusicTheoryConnectionString))
            {
                cnn.Open();

                using (var t = cnn.BeginTransaction())
                {
                    lesson.Id = _repository.MergeLesson(lesson, cnn, t);

                    foreach (var question in lesson.Questions)
                    {
                        question.Id = _repository.MergeQuestion(cnn, t, question);

                        _repository.MergeLessonQuestion(lesson.Id, cnn, t, question.Id);

                        foreach (var textOption in question.Options)
                        {
                            textOption.Id = _optionRepositoryFactory.CreateRepository(textOption.TypeId).MergeOption(cnn, t, textOption);

                            _repository.MergeQuestionOption(cnn, t, question.Id, textOption);
                        }
                    }

                    t.Commit();

                }
            }

            return lesson.Id;
        }

        public void DeleteOption(int questionId, int optionId, OptionType typeId)
        {
            using (var cnn = new SqlConnection(_config.ConnectionStrings.MusicTheoryConnectionString))
            {
                cnn.Open();

                using (var t = cnn.BeginTransaction())
                {
                    DeleteOption(cnn, t, questionId, optionId, typeId);

                    t.Commit();
                }
            }
        }

        private void DeleteOption(SqlConnection cnn, SqlTransaction t, int questionId, int optionId, OptionType typeId)
        {
            _repository.DeleteQuestionOptions(cnn, t, questionId, optionId);
            if (!_repository.IsOptionUsedByAnyQuestions(cnn, t, optionId))
            {
                _optionRepositoryFactory.CreateRepository(typeId).DeleteOption(cnn, t, optionId);
            }
        }

        public void DeleteQuestion(int lessonId, int questionId)
        {
            using (var cnn = new SqlConnection(_config.ConnectionStrings.MusicTheoryConnectionString))
            {
                cnn.Open();

                using (var t = cnn.BeginTransaction())
                {
                    DeleteQuestion(cnn, t, lessonId, questionId);

                    t.Commit();
                }
            }
        }

        private void DeleteQuestion(SqlConnection cnn, SqlTransaction t, int lessonId, int questionId)
        {
            var options = _repository.GetSkeletonOptions(cnn, t, questionId);
            foreach (var option in options)
            {
                DeleteOption(cnn, t, questionId, option.Id, option.TypeId);
            }
            _repository.DeleteLessonQuestions(cnn, t, lessonId, questionId);
            if (!_repository.IsQuestionUsedByAnyLessons(cnn, t, questionId))
            {
                _repository.DeleteQuestion(cnn, t, questionId);
            }
        }

        public void DeleteLesson(int lessonId)
        {
            using (var cnn = new SqlConnection(_config.ConnectionStrings.MusicTheoryConnectionString))
            {
                cnn.Open();

                using (var t = cnn.BeginTransaction())
                {
                    var questions = _repository.GetQuestionsForLesson(lessonId, int.MaxValue, cnn, t);

                    foreach (var question in questions)
                    {
                        DeleteQuestion(cnn, t, lessonId, question.Id);
                    }

                    _repository.DeleteLesson(cnn, t, lessonId);

                    t.Commit();
                }
            }
        }

        public List<SelectItem> GetExistingOptions()
        {
            List<SelectItem> options = new List<SelectItem>();
            using (var cnn = new SqlConnection(_config.ConnectionStrings.MusicTheoryConnectionString))
            {
                cnn.Open();

                using (var t = cnn.BeginTransaction())
                {
                    options.AddRange(_optionRepositoryFactory.CreateRepository(OptionType.Text).GetOptions(cnn, t));
                    options.AddRange(_optionRepositoryFactory.CreateRepository(OptionType.Image).GetOptions(cnn, t));
                }
            }

            return options;
        }
    }
}
