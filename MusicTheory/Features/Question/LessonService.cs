using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MusicTheory.Features.Question.Models;

namespace MusicTheory.Features.Question
{
    public interface ILessonService
    {
        Lesson GetLesson(int id);
    }
    public class LessonService : ILessonService
    {
        private readonly ILessonRepository _repository;

        public LessonService(ILessonRepository repository)
        {
            _repository = repository;
        }
        public Lesson GetLesson(int id)
        {
            return _repository.GetLesson(id);
        }
    }
}
