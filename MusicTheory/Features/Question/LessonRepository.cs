using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MusicTheory.Configuration;
using MusicTheory.Features.Question.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
        private readonly MusicTheoryConfiguration _config;
        public LessonRepository(IOptions<MusicTheoryConfiguration> config)
        {
            _config = config.Value;
        }

        public Lesson GetLesson(int lessonId)
        {
            using (var cnn = new SqlConnection(_config.ConnectionStrings.MusicTheoryConnectionString))
            {
                cnn.Open();
                using(var t = cnn.BeginTransaction())
                {
                    var sql = @"
select * from Lessons
";

                    var x = cnn.Query<Lesson>(sql, transaction: t).FirstOrDefault();
                }
            }

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
