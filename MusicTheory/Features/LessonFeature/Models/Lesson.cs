using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicTheory.Features.LessonFeature.Models
{
    public class Lesson
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<Question> Questions { get; set; }
    }
}
