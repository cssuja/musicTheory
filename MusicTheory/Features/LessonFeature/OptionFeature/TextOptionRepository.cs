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
        public void DeleteOption(SqlConnection cnn, SqlTransaction t, int optionId)
        {
            var deleteOptionSql = @"
delete from TextOptions
where Id = @optionId;
";
            cnn.Execute(deleteOptionSql, new { optionId }, transaction: t);
        }

        public object GetOption(SqlConnection cnn, SqlTransaction t, int optionId)
        {
            var textOptionSql = @"
select TextOptions.Text from TextOptions
where Id = @optionId
";
            var option = cnn.Query<string>(textOptionSql, new { optionId }, transaction: t).First();

            return option;
        }


        public int MergeOption(SqlConnection cnn, SqlTransaction t, QuestionOption option)
        {
            var textOptionSql = @"
MERGE INTO TextOptions
     USING (SELECT @Id    AS vId,
                   @Text      AS vText) p
        ON (Id = vId)
WHEN MATCHED
THEN
    UPDATE SET Text = vText
WHEN NOT MATCHED
THEN
    INSERT     (Text)
        VALUES (vText)
OUTPUT inserted.Id;
";
            return cnn.Query<int>(textOptionSql, new {text= option.Option.ToString(), option.Id }, t).Single();
        }
    }
}
