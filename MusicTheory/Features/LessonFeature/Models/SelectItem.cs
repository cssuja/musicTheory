using MusicTheory.Features.LessonFeature.OptionFeature;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicTheory.Features.LessonFeature.Models
{
    public class SelectItem
    {
        public int Id { get; set; }
        public object Display { get; set; }
        public OptionType TypeId { get; set; }
    }
}
