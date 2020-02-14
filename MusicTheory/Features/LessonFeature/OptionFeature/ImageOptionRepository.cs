using Dapper;
using MusicTheory.Features.LessonFeature.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace MusicTheory.Features.LessonFeature.OptionFeature
{
    public class ImageOptionRepository : IOptionRepository
    {
        public void DeleteOption(SqlConnection cnn, SqlTransaction t, int optionId)
        {
            var deleteOptionSql = @"
delete from ImageOptions
where Id = @optionId;
";
            cnn.Execute(deleteOptionSql, new { optionId }, transaction: t);
        }

        public object GetOption(SqlConnection cnn, SqlTransaction t, int optionId)
        {
            var imageOptionSql = @"
select ImageOptions.Image from ImageOptions
where Id = @optionId
";
            var option = cnn.Query<byte[]>(imageOptionSql, new { optionId }, transaction: t).First();

            return Convert.ToBase64String(option);
        }


        public int MergeOption(SqlConnection cnn, SqlTransaction t, QuestionOption option)
        {
            var imageOptionSql = @"
MERGE INTO ImageOptions
    USING (SELECT @Id    AS vId,
                @Image      AS vImage) p
    ON (Id = vId)
WHEN MATCHED
THEN
UPDATE SET Image = vImage
WHEN NOT MATCHED
THEN
INSERT     (Image)
    VALUES (vImage)
OUTPUT inserted.Id;
";

            return cnn.Query<int>(imageOptionSql, new { image = Convert.FromBase64String(option.Option.ToString()), option.Id }, t).Single();
        }
    }
}
