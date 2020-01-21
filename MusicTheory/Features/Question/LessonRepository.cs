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
        void InsertQuestionOption(SqlConnection cnn, SqlTransaction t, int questionId, QuestionOption option);
        List<QuestionOption> GetOptionsForQuestion(SqlConnection cnn, SqlTransaction t, QuestionModel question);
        IList<QuestionModel> GetQuestionsForLesson(int lessonId, int maxNumberOfQuestions, SqlConnection cnn, SqlTransaction t);
        void UpdateQuestionCorrectAnswer(SqlConnection cnn, SqlTransaction t, int questionId, int answerId);
    }
    public class LessonRepository : ILessonRepository
    {
        public List<QuestionOption> GetOptionsForQuestion(SqlConnection cnn, SqlTransaction t, QuestionModel question)
        {
            var textOptionsSql = @"
select TextOptions.Id, TextOptions.Text as ""Option"", QuestionOptions.IsCorrectAnswer from TextOptions 
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

            return options;
        }

        public IList<QuestionModel> GetQuestionsForLesson(int lessonId, int maxNumberOfQuestions, SqlConnection cnn, SqlTransaction t)
        {
            IList<QuestionModel> questions;
            var questionSql = @"
select top (@maxNumberOfQuestions) Id, Text, TypeId from Questions
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
                 Insert into Lessons (Name) values (@lessonName);
        SELECT CAST(SCOPE_IDENTITY() as int)";

            return cnn.Query<int>(lessonSql, new { lessonName }, t).Single();
        }

        public int InsertQuestion(SqlConnection cnn, SqlTransaction t, QuestionModel question)
        {
            var questionSql = @"
Insert into Questions(Text, TypeId) values(@text, @TypeId);
 SELECT CAST(SCOPE_IDENTITY() as int)
";
            return cnn.Query<int>(questionSql, new { question.Text, question.TypeId }, t).Single();
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

        public void InsertQuestionOption(SqlConnection cnn, SqlTransaction t, int questionId, QuestionOption option)
        {
            var questionOptionsSql = @"
Insert into QuestionOptions(QuestionId, OptionId, IsCorrectAnswer) values(@questionId,  @optionId, @optionIsCorrect);
";
            cnn.Execute(questionOptionsSql, new { questionId, optionId = option.Id, optionIsCorrect = option.IsCorrectAnswer }, t);
        }

        public void UpdateQuestionCorrectAnswer(SqlConnection cnn, SqlTransaction t, int questionId, int answerId)
        {
            var questionOptionsSql = @"
update Questions set AnswerOptionId = @answerId
where Id = @questionId;
";
            cnn.Execute(questionOptionsSql, new { answerId, questionId }, t);
        }
    }
}
