using MusicTheory.Features.Question.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicTheory.Features.Question
{
    public interface IQuestionRepository
    {
        QuestionModel GetQuestion(int id);
    }
    public class QuestionRepository : IQuestionRepository
    {
        public QuestionRepository()
        {

        }

        public QuestionModel GetQuestion(int id)
        {
            var options = new List<IQuestionOption> {
                new TextQuestionOption {
                    Id = 1,
                    QuestionId = id,
                    Text="Option1"
                },
                new ImageQuestionOption {
                    Id = 2,
                    QuestionId = id,
                    Image = new byte[0]
                }
            };

            return new QuestionModel
            {
                Id = id,
                QuestionText = "This is the first question",
                Options = options
            };
        }
    }
}
