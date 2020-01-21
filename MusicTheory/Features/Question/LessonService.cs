using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MusicTheory.Configuration;
using MusicTheory.Features.Question.Models;

namespace MusicTheory.Features.Question
{
    public interface ILessonService
    {
        Lesson GetLesson(int id, int maxNumberOfQuestions);
        List<Lesson> GetLessons();
        void MergeLesson(Lesson lesson);
    }
    public class LessonService : ILessonService
    {
        private readonly ILessonRepository _repository;
        private readonly MusicTheoryConfiguration _config;

        public LessonService(ILessonRepository repository, IOptions<MusicTheoryConfiguration> config)
        {
            _repository = repository;
            _config = config.Value;
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
                        question.Options = _repository.GetOptionsForQuestion(cnn, t, question);
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

        public void MergeLesson(Lesson lesson)
        {
            using (var cnn = new SqlConnection(_config.ConnectionStrings.MusicTheoryConnectionString))
            {
                cnn.Open();

                using (var t = cnn.BeginTransaction())
                {
                    lesson.Id = _repository.InsertLesson(lesson.Name, cnn, t);

                    foreach (var question in lesson.Questions)
                    {
                        question.Id = _repository.InsertQuestion(cnn, t, question);

                        _repository.InsertLessonQuestion(lesson.Id, cnn, t, question.Id);

                        foreach (var textOption in question.Options)
                        {
                            textOption.Id = _repository.InsertTextOption(cnn, t, (string)textOption.Option);

                            _repository.InsertQuestionOption(cnn, t, question.Id, textOption);
                        }
                    }

                    t.Commit();

                }
            }
        }
    }
}
