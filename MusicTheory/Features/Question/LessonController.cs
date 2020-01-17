﻿using Microsoft.AspNetCore.Mvc;
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



    }
}
