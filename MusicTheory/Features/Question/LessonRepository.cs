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
        Lesson GetLesson(int lessonId, SqlConnection cnn, SqlTransaction t);
        List<Lesson> GetLessons(SqlConnection cnn, SqlTransaction t);
        int InsertLesson(string lessonName, SqlConnection cnn, SqlTransaction t);
        int InsertQuestion(SqlConnection cnn, SqlTransaction t, QuestionModel question);
        void InsertLessonQuestion(int lessonId, SqlConnection cnn, SqlTransaction t, int questionId);
        int InsertTextOption(SqlConnection cnn, SqlTransaction t, string text);
        void InsertQuestionOption(SqlConnection cnn, SqlTransaction t, int questionId, int textOptionId);
        List<QuestionOption> GetOptionsForQuestion(SqlConnection cnn, SqlTransaction t, QuestionModel question);
        IList<QuestionModel> GetQuestionsForLesson(int lessonId, int maxNumberOfQuestions, SqlConnection cnn, SqlTransaction t);
    }
    public class LessonRepository : ILessonRepository
    {
        public List<QuestionOption> GetOptionsForQuestion(SqlConnection cnn, SqlTransaction t, QuestionModel question)
        {
            var correctAnswerIdSql = @"
select AnswerOptionId from Questions
where Id = @questionId
";
            var answerId = cnn.Query<int>(correctAnswerIdSql, new { questionId = question.Id }, transaction: t).FirstOrDefault();

            var textOptionsSql = @"
select TextOptions.Id, TextOptions.Text as ""Option"" from TextOptions 
inner join QuestionOptions on TextOptions.Id = QuestionOptions.OptionId
where QuestionOptions.QuestionId = @questionId
";
            string sql = "";

            switch (question.TypeId)
            {
                case 1:
                    {
                        sql = textOptionsSql;
                        break;
                    }
            }

            var options = cnn.Query<QuestionOption>(sql, new { questionId = question.Id }, transaction: t).ToList();

            options.FirstOrDefault(o => o.Id == answerId).IsCorrectAnswer = true;

            return options;
        }

        public IList<QuestionModel> GetQuestionsForLesson(int lessonId, int maxNumberOfQuestions, SqlConnection cnn, SqlTransaction t)
        {
            IList<QuestionModel> questions;
            var questionSql = @"
select top (@maxNumberOfQuestions) Id, Text as QuestionText, AnswerOptionId as AnswerId, TypeId from Questions
where Id in (select QuestionId from LessonQuestions where LessonId = @lessonId)
";
            questions = cnn.Query<QuestionModel>(questionSql, new { lessonId, maxNumberOfQuestions }, transaction: t).ToList();
            return questions;
        }

        public Lesson GetLesson(int lessonId, SqlConnection cnn, SqlTransaction t)
        {
            Lesson lesson;
            var lessonSql = @"
select * from Lessons
where Id = @lessonId
";

            lesson = cnn.Query<Lesson>(lessonSql, new { lessonId }, transaction: t).FirstOrDefault();
            return lesson;
        }

        public List<Lesson> GetLessons(SqlConnection cnn, SqlTransaction t)
        {
            List<Lesson> lessons;
            var lessonSql = @"
select * from Lessons
";

            lessons = cnn.Query<Lesson>(lessonSql, transaction: t).ToList();
            return lessons;
        }

        public int InsertLesson(string lessonName, SqlConnection cnn, SqlTransaction t)
        {
            var lessonSql = @"
                 Insert into Lessons (Name) values (@name);
        SELECT CAST(SCOPE_IDENTITY() as int)";

            return cnn.Query<int>(lessonSql, new { lessonName }, t).Single();
        }

        public int InsertQuestion(SqlConnection cnn, SqlTransaction t, QuestionModel question)
        {
            var questionSql = @"
Insert into Questions(Text, TypeId) values(@QuestionText, @TypeId);
 SELECT CAST(SCOPE_IDENTITY() as int)
";
            return cnn.Query<int>(questionSql, new { question.QuestionText, question.TypeId }, t).Single();
        }

        public void InsertLessonQuestion(int lessonId, SqlConnection cnn, SqlTransaction t, int questionId)
        {
            var lessonQuestionSql = @"
Insert into LessonQuestions(LessonId, QuestionId) values(@lessonId,  @questionId);
";
            cnn.Execute(lessonQuestionSql, new { lessonId, questionId }, t);
        }

        public int InsertTextOption(SqlConnection cnn, SqlTransaction t, string text)
        {
            var textOptionSql = @"
Insert into TextOptions(Text) values(@text);
 SELECT CAST(SCOPE_IDENTITY() as int)
";
            return cnn.Query<int>(textOptionSql, new { text }, t).Single();
        }

        public void InsertQuestionOption(SqlConnection cnn, SqlTransaction t, int questionId, int textOptionId)
        {
            var questionOptionsSql = @"
Insert into QuestionOptions(QuestionId, OptionId) values(@questionId,  @optionId);
";
            cnn.Execute(questionOptionsSql, new { questionId, optionId = textOptionId }, t);
        }
    }
}
