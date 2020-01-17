using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicTheory.Features.Question.Models
{
    public class QuestionModel
    {
        public int Id { get; set; }
        public string QuestionText { get; set; }
        public int AnswerId { get; set; }
        public List<TextQuestionOption> TextOptions { get; set; }
        public List<ImageQuestionOption> ImageOptions { get; set; }
        public int TypeId { get; set; }
    }
}
