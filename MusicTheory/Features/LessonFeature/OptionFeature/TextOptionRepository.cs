using Dapper;
using MusicTheory.Features.LessonFeature.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MusicTheory.Features.LessonFeature.OptionFeature
{
    public class TextOptionRepository : IOptionRepository
    {
        public List<QuestionOption> GetOptionsForQuestion(SqlConnection cnn, SqlTransaction t, Question question)
        {
            var textOptionsSql = @"
select TextOptions.Id, TextOptions.Text as ""Option"", QuestionOptions.IsCorrectAnswer from TextOptions 
inner join QuestionOptions on TextOptions.Id = QuestionOptions.OptionId
where QuestionOptions.QuestionId = @questionId
";
            string sql = "";

            switch (question.TypeId)
            {
                case OptionType.Text:
                    {
                        sql = textOptionsSql;
                        break;
                    }
            }

            var options = cnn.Query<QuestionOption>(sql, new { questionId = question.Id }, transaction: t).ToList();

            return options;
        }


        public int InsertOption(SqlConnection cnn, SqlTransaction t, object text)
        {
            var textOptionSql = @"
Insert into TextOptions(Text) values(@text);
 SELECT CAST(SCOPE_IDENTITY() as int)
";
            return cnn.Query<int>(textOptionSql, new {text= text.ToString() }, t).Single();
        }
    }
}
