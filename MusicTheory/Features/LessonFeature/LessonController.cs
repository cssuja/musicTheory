using Microsoft.AspNetCore.Mvc;
using MusicTheory.Features.LessonFeature;
using MusicTheory.Features.LessonFeature.Models;
using MusicTheory.Features.LessonFeature.OptionFeature;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicTheory.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LessonController : ControllerBase
    {
        private readonly ILessonService _service;

        public LessonController(ILessonService service)
        {
            _service = service;
        }
        [HttpGet]
        public Lesson Get([FromQuery] int id, [FromQuery] int maxNoOfQuestions)
        {
            return _service.GetLesson(id, maxNoOfQuestions);
        }

        [HttpGet("Lessons")]
        public List<Lesson> Get()
        {
            return _service.GetLessons();
        }

        [HttpPut]
        public int Put([FromBody] Lesson lesson)
        {
            return _service.MergeLesson(lesson);
        }

        [HttpPut("Question")]
        public int Put([FromBody] Question question, [FromQuery] int lessonId)
        {
            return _service.MergeQuestion(question, lessonId);
        }

        [HttpPut("Option")]
        public int Put([FromBody] QuestionOption option, [FromQuery] int questionId)
        {
            return _service.MergeOption(option, questionId);
        }

        [HttpDelete("Option")]
        public void Delete([FromQuery] int questionId, [FromQuery] int optionId, [FromQuery] OptionType typeId)
        {
            _service.DeleteOption(questionId, optionId, typeId);
        }
    }
}
