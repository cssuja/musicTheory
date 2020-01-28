using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MusicTheory.Configuration;
using MusicTheory.Features.LessonFeature.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MusicTheory.Features.LessonFeature
{
    public interface ILessonRepository
    {
        Lesson GetLesson(int lessonId, SqlConnection cnn, SqlTransaction t);
        List<Lesson> GetLessons(SqlConnection cnn, SqlTransaction t);
        int MergeLesson(Lesson lesson, SqlConnection cnn, SqlTransaction t);
        int InsertQuestion(SqlConnection cnn, SqlTransaction t, Question question);
        void InsertLessonQuestion(int lessonId, SqlConnection cnn, SqlTransaction t, int questionId);
        void InsertQuestionOption(SqlConnection cnn, SqlTransaction t, int questionId, QuestionOption option);
        IList<Models.Question> GetQuestionsForLesson(int lessonId, int maxNumberOfQuestions, SqlConnection cnn, SqlTransaction t);
    }
    public class LessonRepository : ILessonRepository
    {


        public IList<Question> GetQuestionsForLesson(int lessonId, int maxNumberOfQuestions, SqlConnection cnn, SqlTransaction t)
        {
            IList<Models.Question> questions;
            var questionSql = @"
select top (@maxNumberOfQuestions) Id, Text, TypeId from Questions
where Id in (select QuestionId from LessonQuestions where LessonId = @lessonId)
";
            questions = cnn.Query<Question>(questionSql, new { lessonId, maxNumberOfQuestions }, transaction: t).ToList();
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

        public int MergeLesson(Lesson lesson, SqlConnection cnn, SqlTransaction t)
        {
            var lessonSql = @"
MERGE INTO Lessons
     USING (SELECT @Id    AS vId,
                   @Name      AS vName) p
        ON (Id = vId)
WHEN MATCHED
THEN
    UPDATE SET Name = vName
WHEN NOT MATCHED
THEN
    INSERT     (Name)
        VALUES (vName)
OUTPUT inserted.Id;
";

            return cnn.Query<int>(lessonSql, new { lesson.Name, lesson.Id }, t).Single();
        }

        public int InsertQuestion(SqlConnection cnn, SqlTransaction t, Question question)
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


        public void InsertQuestionOption(SqlConnection cnn, SqlTransaction t, int questionId, QuestionOption option)
        {
            var questionOptionsSql = @"
Insert into QuestionOptions(QuestionId, OptionId, IsCorrectAnswer) values(@questionId,  @optionId, @optionIsCorrect);
";
            cnn.Execute(questionOptionsSql, new { questionId, optionId = option.Id, optionIsCorrect = option.IsCorrectAnswer }, t);
        }
    }
}
