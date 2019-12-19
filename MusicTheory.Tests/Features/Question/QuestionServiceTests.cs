using FluentAssertions;
using Moq;
using MusicTheory.Features.Question;
using MusicTheory.Features.Question.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MusicTheory.Tests.Features.Question
{
   public class QuestionServiceTests
    {
        private readonly Mock<IQuestionRepository> _mockRepository;
        private readonly IQuestionService  _service;

        public QuestionServiceTests()
        {
            _mockRepository = new Mock<IQuestionRepository>();
            _service = new QuestionService(_mockRepository.Object);
        }

        [Fact]
        public void GetQuestion_Should_Return_Correct_Question_From_Repository()
        {
            var expectedLesson = new Lesson
            {
                Id = 2,
                Name = "Major Scale",
                Questions = new List<QuestionModel> {
                    new QuestionModel {
                        Id = 5,
                        QuestionText = "This is the first question"
                    }
                }
            };

            _mockRepository.Setup(x => x.GetLesson(expectedLesson.Id)).Returns(expectedLesson);
            var actualLesson = _service.GetLesson(expectedLesson.Id);
            actualLesson.Should().Be(expectedLesson);

        }
    }
}
