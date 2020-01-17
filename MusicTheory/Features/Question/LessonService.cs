using System;
using System.Collections.Generic;
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
            var lesson = _repository.GetLesson(id, maxQuestions);
            lesson.Questions= lesson.Questions.OrderBy(q => random.Next()).ToList();
            foreach(var question in lesson.Questions)
            {
                question.TextOptions = question.TextOptions.OrderBy(q => random.Next()).ToList();
            }
            return lesson ;
        }

        public List<Lesson> GetLessons()
        {
            return _repository.GetLessons();
        }

        public void MergeLesson(Lesson lesson)
        {
            _repository.MergeLesson(lesson);
        }
    }
}
