using MusicTheory.Features.Question.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicTheory.Features.Question
{
    public interface IQuestionRepository
    {
        Lesson GetLesson(int id);
    }
    public class QuestionRepository : IQuestionRepository
    {
        public QuestionRepository()
        {

        }

        public Lesson GetLesson(int lessonId)
        {
            var options = new List<TextQuestionOption> {
                new TextQuestionOption {
                    Id = 1,
                    QuestionId = lessonId,
                    Text="Option1"
                },
                new TextQuestionOption {
                    Id = 3,
                    QuestionId = lessonId,
                    Text="Option2"
                },
                new TextQuestionOption {
                    Id = 4,
                    QuestionId = lessonId,
                    Text="Option3"
                },
            };

            var lesson = new Lesson
            {
                Id = lessonId,
                Name = "Major Scale",
                Questions = new List<QuestionModel> {
                    new QuestionModel
                    {
                        Id = lessonId,
                        QuestionText = "This is the first question",
                        TextOptions = options,
                        AnswerId = 3
                    }
                }
            };

            return lesson;
        }
    }
}
