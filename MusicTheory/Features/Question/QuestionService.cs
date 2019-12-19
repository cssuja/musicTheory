using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MusicTheory.Features.Question.Models;

namespace MusicTheory.Features.Question
{
    public interface IQuestionService
    {
        Lesson GetLesson(int id);
    }
    public class QuestionService : IQuestionService
    {
        private readonly IQuestionRepository _repository;

        public QuestionService(IQuestionRepository repository)
        {
            _repository = repository;
        }
        public Lesson GetLesson(int id)
        {
            return _repository.GetLesson(id);
        }
    }
}
