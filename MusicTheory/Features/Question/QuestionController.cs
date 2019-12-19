using Microsoft.AspNetCore.Mvc;
using MusicTheory.Features.Question;
using MusicTheory.Features.Question.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MusicTheory.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService _service;

        public QuestionController(IQuestionService service)
        {
            _service = service;
        }
        [HttpGet]
        public Lesson Get([FromQuery] int id)
        {
            return _service.GetLesson(id);
        }
    }
}
