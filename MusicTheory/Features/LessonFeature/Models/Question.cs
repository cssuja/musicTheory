using MusicTheory.Features.LessonFeature.OptionFeature;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicTheory.Features.LessonFeature.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public List<QuestionOption> Options { get; set; }
        public OptionType TypeId { get; set; }
    }
}
