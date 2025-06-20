using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Moq;
using QuizupAPI.Services;
using QuizupAPI.Interfaces;
using QuizupAPI.Models;
using QuizupAPI.Models.DTOs.Quiz;
using QuizupAPI.Models.DTOs.Question;
using QuizupAPI.Repositories;
using QuizupAPI.Misc.Mappers;

namespace QuizupAPI.Test.Services
{
    public class QuizServiceTest
    {
        private readonly Mock<IRepository<long, Quiz>> _quizRepoMock = new();
        private readonly Mock<IRepository<long, Question>> _questionRepoMock = new();
        private readonly Mock<IRepository<long, QuizQuestion>> _quizQuestionRepoMock = new();

        private readonly QuizService _service;

        public QuizServiceTest()
        {
            _service = new QuizService(
                _quizRepoMock.Object,
                _questionRepoMock.Object,
                _quizQuestionRepoMock.Object
            );
        }

        [Fact]
        public async Task AddQuizAsync_ShouldThrow_WhenDtoIsNull()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _service.AddQuizAsync(null));
        }

        [Fact]
        public async Task AddQuizAsync_ShouldThrow_WhenRequiredFieldsMissing()
        {
            var dto = new QuizAddRequestDTO { Title = "", Description = "" };
            await Assert.ThrowsAsync<ValidationException>(() => _service.AddQuizAsync(dto));
        }

        [Fact]
        public async Task AddQuizAsync_ShouldThrow_WhenAddFails()
        {
            var dto = new QuizAddRequestDTO { Title = "Quiz", Description = "Desc", Questions = new List<QuestionAddRequestDTO> { new QuestionAddRequestDTO { Text = "Q", Options = new List<string> { "A" }, CorrectAnswer = "A" } } };
            _quizRepoMock.Setup(r => r.Add(It.IsAny<Quiz>())).ReturnsAsync((Quiz)null);

            await Assert.ThrowsAsync<Exception>(() => _service.AddQuizAsync(dto));
        }

        [Fact]
        public async Task GetQuizByIdAsync_ShouldReturnQuiz()
        {
            var quiz = new Quiz { Id = 1, Title = "Quiz" };
            _quizRepoMock.Setup(r => r.Get(1)).ReturnsAsync(quiz);

            var result = await _service.GetQuizByIdAsync(1);

            Assert.Equal(quiz, result);
        }

        [Fact]
        public async Task GetQuizByIdAsync_ShouldThrow_WhenNotFound()
        {
            _quizRepoMock.Setup(r => r.Get(It.IsAny<long>())).ThrowsAsync(new KeyNotFoundException("not found"));
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.GetQuizByIdAsync(99));
        }

        [Fact]
        public async Task GetAllQuizzesAsync_ShouldReturnQuizzes()
        {
            var quizzes = new List<Quiz> { new Quiz { Id = 1 }, new Quiz { Id = 2 } };
            _quizRepoMock.Setup(r => r.GetAll()).ReturnsAsync(quizzes);

            var result = await _service.GetAllQuizzesAsync();

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task UpdateQuizAsync_ShouldThrow_WhenDtoIsNull()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _service.UpdateQuizAsync(1, 1, null));
        }

        [Fact]
        public async Task UpdateQuizAsync_ShouldThrow_WhenRequiredFieldsMissing()
        {
            var dto = new QuizUpdateRequestDTO { Title = "", Description = "" };
            await Assert.ThrowsAsync<ArgumentException>(() => _service.UpdateQuizAsync(1, 1, dto));
        }

        [Fact]
        public async Task UpdateQuizAsync_ShouldThrow_WhenNotFound()
        {
            _quizRepoMock.Setup(r => r.Get(It.IsAny<long>())).ThrowsAsync(new KeyNotFoundException("not found"));
            var dto = new QuizUpdateRequestDTO { Title = "Quiz", Description = "Desc" };
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.UpdateQuizAsync(1, 1, dto));
        }

        [Fact]
        public async Task UpdateQuizAsync_ShouldThrow_WhenUnauthorized()
        {
            var quiz = new Quiz { Id = 1, TeacherId = 2 };
            _quizRepoMock.Setup(r => r.Get(1)).ReturnsAsync(quiz);
            var dto = new QuizUpdateRequestDTO { Title = "Quiz", Description = "Desc" };

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _service.UpdateQuizAsync(1, 1, dto));
        }

        [Fact]
        public async Task DeleteQuizAsync_ShouldThrow_WhenNotFound()
        {
            _quizRepoMock.Setup(r => r.Get(It.IsAny<long>())).ThrowsAsync(new KeyNotFoundException("not found"));
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.DeleteQuizAsync(1, 1));
        }

        [Fact]
        public async Task DeleteQuizAsync_ShouldThrow_WhenUnauthorized()
        {
            var quiz = new Quiz { Id = 1, TeacherId = 2 };
            _quizRepoMock.Setup(r => r.Get(1)).ReturnsAsync(quiz);

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _service.DeleteQuizAsync(1, 1));
        }

        [Fact]
        public async Task AddQuestionToQuizAsync_ShouldThrow_WhenDtoIsNull()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _service.AddQuestionToQuizAsync(1, 1, null));
        }

        [Fact]
        public async Task AddQuestionToQuizAsync_ShouldThrow_WhenRequiredFieldsMissing()
        {
            var dto = new QuestionAddRequestDTO { Text = "", Options = new List<string>() };
            await Assert.ThrowsAsync<ValidationException>(() => _service.AddQuestionToQuizAsync(1, 1, dto));
        }

        [Fact]
        public async Task AddQuestionToQuizAsync_ShouldThrow_WhenMapFails()
        {
            var questionMapperMock = new Mock<QuestionMapper>();
            questionMapperMock.Setup(m => m.MapQuestionAddRequestQuestion(It.IsAny<QuestionAddRequestDTO>())).Returns((Question)null);

            var service = new QuizService(_quizRepoMock.Object, _questionRepoMock.Object, _quizQuestionRepoMock.Object);
            service.questionMapper = questionMapperMock.Object;

            var dto = new QuestionAddRequestDTO { Text = "Q", Options = new List<string> { "A" }, CorrectAnswer = "A" };
            await Assert.ThrowsAsync<Exception>(() => service.AddQuestionToQuizAsync(1, 1, dto));
        }

        [Fact]
        public async Task UpdateQuestionAsync_ShouldThrow_WhenDtoIsNull()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _service.UpdateQuestionAsync(1, 1, null));
        }

        [Fact]
        public async Task UpdateQuestionAsync_ShouldThrow_WhenRequiredFieldsMissing()
        {
            var dto = new QuestionUpdateRequestDTO { Text = "", Options = new List<string>(), CorrectAnswer = "" };
            await Assert.ThrowsAsync<ValidationException>(() => _service.UpdateQuestionAsync(1, 1, dto));
        }

        [Fact]
        public async Task GetQuizzesPaginationAsync_ShouldThrow_WhenInvalidPage()
        {
            await Assert.ThrowsAsync<Exception>(() => _service.GetQuizzesPaginationAsync(0, 10));
            await Assert.ThrowsAsync<Exception>(() => _service.GetQuizzesPaginationAsync(1, 0));
        }

        [Fact]
        public async Task GetQuizzesPaginationAsync_ShouldReturnPaged()
        {
            var quizzes = Enumerable.Range(1, 20).Select(i => new Quiz { Id = i }).ToList();
            _quizRepoMock.Setup(r => r.GetAll()).ReturnsAsync(quizzes);

            var result = await _service.GetQuizzesPaginationAsync(2, 5);

            Assert.Equal(5, result.Count());
            Assert.Equal(6, result.First().Id);
        }
    }
}