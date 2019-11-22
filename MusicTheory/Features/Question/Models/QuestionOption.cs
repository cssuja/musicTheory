using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicTheory.Features.Question.Models
{
    public interface IQuestionOption
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
    }
    public class TextQuestionOption :  IQuestionOption
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string Text { get; set; }
    }
    public class ImageQuestionOption : IQuestionOption
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public byte[] Image { get; set; }
    }
}
