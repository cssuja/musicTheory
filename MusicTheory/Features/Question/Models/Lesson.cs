using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicTheory.Features.Question.Models
{
    public class Lesson
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<QuestionModel> Questions { get; set; }
    }
}
