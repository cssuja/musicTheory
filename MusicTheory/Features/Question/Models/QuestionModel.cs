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
        public List<QuestionOption> Options { get; set; }
        public int TypeId { get; set; }
    }
}
