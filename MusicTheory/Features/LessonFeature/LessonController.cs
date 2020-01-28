using Microsoft.AspNetCore.Mvc;
using MusicTheory.Features.LessonFeature;
using MusicTheory.Features.LessonFeature.Models;
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

        //[HttpPost]
        //public int Post([FromBody] Lesson lesson)
        //{
        //    return _service.InsertLesson(lesson);
        //}

        [HttpPost("Question")]
        public int Post([FromBody] Question question)
        {
            return _service.InsertQuestion(question);
        }

        [HttpPost("Option")]
        public int Post([FromBody] QuestionOption option)
        {
            return _service.InsertOption(option);
        }

    }
}
