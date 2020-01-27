using MusicTheory.Features.LessonFeature.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace MusicTheory.Features.LessonFeature.OptionFeature
{
    public interface IOptionRepository
    {
        int InsertOption(SqlConnection cnn, SqlTransaction t, object text);
        List<QuestionOption> GetOptionsForQuestion(SqlConnection cnn, SqlTransaction t, Question question);
    }
}
