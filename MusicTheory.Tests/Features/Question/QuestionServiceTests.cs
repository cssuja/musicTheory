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
            var expectedQuestion = new QuestionModel { Id = 5, QuestionText = "This is the first question" };
            _mockRepository.Setup(x => x.GetQuestion(expectedQuestion.Id)).Returns(expectedQuestion);
            var actualQuestion = _service.GetQuestion(expectedQuestion.Id);
            actualQuestion.Should().Be(expectedQuestion);

        }
    }
}
