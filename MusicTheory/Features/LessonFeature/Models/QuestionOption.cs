using MusicTheory.Features.LessonFeature.OptionFeature;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicTheory.Features.LessonFeature.Models
{
    public class QuestionOption
    {
        public int Id { get; set; }
        public object Option { get; set; }
        public bool IsCorrectAnswer { get; set; }
        public OptionType TypeId { get; set; }
    }
}
