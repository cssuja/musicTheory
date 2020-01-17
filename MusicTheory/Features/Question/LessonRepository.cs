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
        Lesson GetLesson(int id, int maxNumberOfQuestions);
        List<Lesson> GetLessons();
    }
    public class LessonRepository : ILessonRepository
    {
        private readonly MusicTheoryConfiguration _config;
        public LessonRepository(IOptions<MusicTheoryConfiguration> config)
        {
            _config = config.Value;
        }

        public Lesson GetLesson(int lessonId, int maxNumberOfQuestions)
        {
            Lesson lesson;
            IList<QuestionModel> questions;
            using (var cnn = new SqlConnection(_config.ConnectionStrings.MusicTheoryConnectionString))
            {
                cnn.Open();

                using(var t = cnn.BeginTransaction())
                {
                    var lessonSql = @"
select * from Lessons
where Id = @lessonId
";

                    lesson = cnn.Query<Lesson>(lessonSql, new { lessonId }, transaction: t).FirstOrDefault();

                    var questionSql = @"
select top (@maxNumberOfQuestions) Id, Text as QuestionText, AnswerOptionId as AnswerId from Questions
where Id in (select QuestionId from LessonQuestions where LessonId = @lessonId)
";
                    questions = cnn.Query<QuestionModel>(questionSql, new { lessonId, maxNumberOfQuestions }, transaction: t).ToList();

                    var textOptionsSql = @"
select TextOptions.Id, TextOptions.Text from TextOptions 
inner join QuestionOptions on TextOptions.Id = QuestionOptions.OptionId
where QuestionOptions.QuestionId = @questionId
";

                    foreach(var question in questions)
                    {
                        var options = cnn.Query<TextQuestionOption>(textOptionsSql, new { questionId = question.Id }, transaction: t).ToList();
                        question.TextOptions = options;
                    }

                    lesson.Questions = questions;
                }
            }

            //var options = new List<TextQuestionOption> {
            //    new TextQuestionOption {
            //        Id = 1,
            //        QuestionId = lessonId,
            //        Text="Option1"
            //    },
            //    new TextQuestionOption {
            //        Id = 2,
            //        QuestionId = lessonId,
            //        Text="Option2"
            //    },
            //    new TextQuestionOption {
            //        Id = 3,
            //        QuestionId = lessonId,
            //        Text="Option3"
            //    },
            //    new TextQuestionOption {
            //        Id = 4,
            //        QuestionId = lessonId,
            //        Text="Option4"
            //    },
            //};

            //var lesson = new Lesson
            //{
            //    Id = lessonId,
            //    Name = "Major Scale",
            //    Questions = new List<QuestionModel> {
            //        new QuestionModel
            //        {
            //            Id = lessonId,
            //            QuestionText = "This is the first question",
            //            TextOptions = options,
            //            AnswerId = 2
            //        },
            //        new QuestionModel
            //        {
            //            Id = lessonId,
            //            QuestionText = "This is the second question",
            //            TextOptions = options,
            //            AnswerId = 3
            //        },
            //        new QuestionModel
            //        {
            //            Id = lessonId,
            //            QuestionText = "This is the third question",
            //            TextOptions = options,
            //            AnswerId = 1
            //        },
            //        new QuestionModel
            //        {
            //            Id = lessonId,
            //            QuestionText = "This is the fourth question",
            //            TextOptions = options,
            //            AnswerId = 3
            //        },
            //        new QuestionModel
            //        {
            //            Id = lessonId,
            //            QuestionText = "This is the fifth question",
            //            TextOptions = options,
            //            AnswerId = 1
            //        }
            //    }
            //};

            return lesson;
        }

        public List<Lesson> GetLessons()
        {
            List<Lesson> lessons;
            using (var cnn = new SqlConnection(_config.ConnectionStrings.MusicTheoryConnectionString))
            {
                cnn.Open();

                using (var t = cnn.BeginTransaction())
                {
                    var lessonSql = @"
select * from Lessons
";

                    lessons = cnn.Query<Lesson>(lessonSql,  transaction: t).ToList();
                }
            }
            return lessons;
        }
    }
}
