using MusicTheory.Features.LessonFeature.Models;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace MusicTheory.Features.LessonFeature.OptionFeature
{
    public interface IOptionRepository
    {
        int MergeOption(SqlConnection cnn, SqlTransaction t, QuestionOption option);
        object GetOption(SqlConnection cnn, SqlTransaction t, int optionId);
    }
}
