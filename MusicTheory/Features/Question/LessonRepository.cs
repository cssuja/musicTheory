using MusicTheory.Features.Question.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicTheory.Features.Question
{
    public interface ILessonRepository
    {
        Lesson GetLesson(int id);
    }
    public class LessonRepository : ILessonRepository
    {
        public LessonRepository()
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
                    Id = 2,
                    QuestionId = lessonId,
                    Text="Option2"
                },
                new TextQuestionOption {
                    Id = 3,
                    QuestionId = lessonId,
                    Text="Option3"
                },
                new TextQuestionOption {
                    Id = 4,
                    QuestionId = lessonId,
                    Text="Option4"
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
                        AnswerId = 2
                    },
                    new QuestionModel
                    {
                        Id = lessonId,
                        QuestionText = "This is the second question",
                        TextOptions = options,
                        AnswerId = 3
                    },
                    new QuestionModel
                    {
                        Id = lessonId,
                        QuestionText = "This is the third question",
                        TextOptions = options,
                        AnswerId = 1
                    },
                    new QuestionModel
                    {
                        Id = lessonId,
                        QuestionText = "This is the fourth question",
                        TextOptions = options,
                        AnswerId = 3
                    },
                    new QuestionModel
                    {
                        Id = lessonId,
                        QuestionText = "This is the fifth question",
                        TextOptions = options,
                        AnswerId = 1
                    }
                }
            };

            return lesson;
        }
    }
}
