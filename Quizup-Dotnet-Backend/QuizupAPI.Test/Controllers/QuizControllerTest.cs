using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using QuizupAPI.Controllers;
using QuizupAPI.Interfaces;
using QuizupAPI.Models;
using QuizupAPI.Models.DTOs.Quiz;
using QuizupAPI.Models.DTOs.Question;
using QuizupAPI.Models.DTOs.Response;

namespace QuizupAPI.Test.Controllers
{
    public class QuizControllerTest
    {
        private readonly Mock<IQuizService> _quizServiceMock = new();
        private readonly QuizzesController _controller;

        public QuizControllerTest()
        {
            _controller = new QuizzesController(_quizServiceMock.Object);
        }

        [Fact]
        public async Task GetAllQuizzes_ReturnsOk_WithApiResponse()
        {
            var quizzes = new List<Quiz> { new Quiz { Id = 1 }, new Quiz { Id = 2 } };
            _quizServiceMock.Setup(s => s.GetAllQuizzesAsync()).ReturnsAsync(quizzes);

            var result = await _controller.GetAllQuizzes();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<IEnumerable<Quiz>>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(quizzes, response.Data);
        }

        [Fact]
        public async Task GetAllQuizzes_ReturnsServerError_OnException()
        {
            _quizServiceMock.Setup(s => s.GetAllQuizzesAsync()).ThrowsAsync(new Exception("fail"));

            var result = await _controller.GetAllQuizzes();

            var statusResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusResult.StatusCode);
            var response = Assert.IsType<ApiResponse<object>>(statusResult.Value);
            Assert.False(response.Success);
        }

        [Fact]
        public async Task GetQuizById_ReturnsOk_WhenFound()
        {
            var quiz = new Quiz { Id = 1 };
            _quizServiceMock.Setup(s => s.GetQuizByIdAsync(1)).ReturnsAsync(quiz);

            var result = await _controller.GetQuizById(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<Quiz>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(quiz, response.Data);
        }

        [Fact]
        public async Task GetQuizById_ReturnsNotFound_WhenNotFound()
        {
            _quizServiceMock.Setup(s => s.GetQuizByIdAsync(1)).ThrowsAsync(new KeyNotFoundException("not found"));

            var result = await _controller.GetQuizById(1);

            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            var response = Assert.IsType<ApiResponse<object>>(notFound.Value);
            Assert.False(response.Success);
        }

        [Fact]
        public async Task GetQuizById_ReturnsServerError_OnException()
        {
            _quizServiceMock.Setup(s => s.GetQuizByIdAsync(1)).ThrowsAsync(new Exception("fail"));

            var result = await _controller.GetQuizById(1);

            var statusResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusResult.StatusCode);
            var response = Assert.IsType<ApiResponse<object>>(statusResult.Value);
            Assert.False(response.Success);
        }

        [Fact]
        public async Task GetQuizzesPagination_ReturnsOk_WithPaginatedResponse()
        {
            var quizzes = new List<Quiz> { new Quiz { Id = 1 }, new Quiz { Id = 2 } };
            _quizServiceMock.Setup(s => s.GetQuizzesPaginationAsync(1, 10)).ReturnsAsync(quizzes);
            _quizServiceMock.Setup(s => s.GetAllQuizzesAsync()).ReturnsAsync(quizzes);

            var result = await _controller.GetQuizzesPagination(1, 10);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<PaginatedResponse<IEnumerable<Quiz>>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(quizzes, response.Data);
        }

        [Fact]
        public async Task GetQuizzesPagination_ReturnsBadRequest_WhenInvalidPage()
        {
            var result = await _controller.GetQuizzesPagination(0, 0);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<ApiResponse<object>>(badRequest.Value);
            Assert.False(response.Success);
        }

        [Fact]
        public async Task GetQuizzesPagination_ReturnsServerError_OnException()
        {
            _quizServiceMock.Setup(s => s.GetQuizzesPaginationAsync(1, 10)).ThrowsAsync(new Exception("fail"));
            _quizServiceMock.Setup(s => s.GetAllQuizzesAsync()).ReturnsAsync(new List<Quiz>());

            var result = await _controller.GetQuizzesPagination(1, 10);

            var statusResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusResult.StatusCode);
            var response = Assert.IsType<ApiResponse<object>>(statusResult.Value);
            Assert.False(response.Success);
        }

        [Fact]
        public async Task CreateQuiz_ReturnsCreated_WhenValid()
        {
            var dto = new QuizAddRequestDTO { Title = "Quiz", Description = "Desc" };
            var quiz = new Quiz { Id = 1, Title = "Quiz" };
            _quizServiceMock.Setup(s => s.AddQuizAsync(dto)).ReturnsAsync(quiz);

            var result = await _controller.CreateQuiz(dto);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var response = Assert.IsType<ApiResponse<Quiz>>(createdResult.Value);
            Assert.True(response.Success);
            Assert.Equal(quiz, response.Data);
        }

        [Fact]
        public async Task CreateQuiz_ReturnsBadRequest_OnArgumentNullException()
        {
            var dto = new QuizAddRequestDTO();
            _quizServiceMock.Setup(s => s.AddQuizAsync(dto)).ThrowsAsync(new ArgumentNullException());

            var result = await _controller.CreateQuiz(dto);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<ApiResponse<object>>(badRequest.Value);
            Assert.False(response.Success);
        }

        [Fact]
        public async Task CreateQuiz_ReturnsBadRequest_OnArgumentException()
        {
            var dto = new QuizAddRequestDTO();
            _quizServiceMock.Setup(s => s.AddQuizAsync(dto)).ThrowsAsync(new ArgumentException());

            var result = await _controller.CreateQuiz(dto);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<ApiResponse<object>>(badRequest.Value);
            Assert.False(response.Success);
        }

        [Fact]
        public async Task CreateQuiz_ReturnsServerError_OnException()
        {
            var dto = new QuizAddRequestDTO();
            _quizServiceMock.Setup(s => s.AddQuizAsync(dto)).ThrowsAsync(new Exception("fail"));

            var result = await _controller.CreateQuiz(dto);

            var statusResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusResult.StatusCode);
            var response = Assert.IsType<ApiResponse<object>>(statusResult.Value);
            Assert.False(response.Success);
        }

        [Fact]
        public async Task UpdateQuiz_ReturnsOk_WhenValid()
        {
            var dto = new QuizUpdateRequestDTO { Title = "Quiz", Description = "Desc" };
            var quiz = new Quiz { Id = 1, Title = "Quiz" };
            _quizServiceMock.Setup(s => s.UpdateQuizAsync(1, 1, dto)).ReturnsAsync(quiz);

            var result = await _controller.UpdateQuiz(1, 1, dto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<Quiz>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(quiz, response.Data);
        }

        [Fact]
        public async Task UpdateQuiz_ReturnsBadRequest_OnArgumentNullException()
        {
            var dto = new QuizUpdateRequestDTO();
            _quizServiceMock.Setup(s => s.UpdateQuizAsync(1, 1, dto)).ThrowsAsync(new ArgumentNullException());

            var result = await _controller.UpdateQuiz(1, 1, dto);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<ApiResponse<object>>(badRequest.Value);
            Assert.False(response.Success);
        }

        [Fact]
        public async Task UpdateQuiz_ReturnsBadRequest_OnArgumentException()
        {
            var dto = new QuizUpdateRequestDTO();
            _quizServiceMock.Setup(s => s.UpdateQuizAsync(1, 1, dto)).ThrowsAsync(new ArgumentException());

            var result = await _controller.UpdateQuiz(1, 1, dto);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<ApiResponse<object>>(badRequest.Value);
            Assert.False(response.Success);
        }

        [Fact]
        public async Task UpdateQuiz_ReturnsNotFound_OnKeyNotFoundException()
        {
            var dto = new QuizUpdateRequestDTO();
            _quizServiceMock.Setup(s => s.UpdateQuizAsync(1, 1, dto)).ThrowsAsync(new KeyNotFoundException());

            var result = await _controller.UpdateQuiz(1, 1, dto);

            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            var response = Assert.IsType<ApiResponse<object>>(notFound.Value);
            Assert.False(response.Success);
        }

        [Fact]
        public async Task UpdateQuiz_ReturnsServerError_OnException()
        {
            var dto = new QuizUpdateRequestDTO();
            _quizServiceMock.Setup(s => s.UpdateQuizAsync(1, 1, dto)).ThrowsAsync(new Exception("fail"));

            var result = await _controller.UpdateQuiz(1, 1, dto);

            var statusResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusResult.StatusCode);
            var response = Assert.IsType<ApiResponse<object>>(statusResult.Value);
            Assert.False(response.Success);
        }

        [Fact]
        public async Task DeleteQuiz_ReturnsOk_WhenValid()
        {
            var quiz = new Quiz { Id = 1 };
            _quizServiceMock.Setup(s => s.DeleteQuizAsync(1, 1)).ReturnsAsync(quiz);

            var result = await _controller.DeleteQuiz(1, 1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<Quiz>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(quiz, response.Data);
        }

        [Fact]
        public async Task DeleteQuiz_ReturnsNotFound_OnKeyNotFoundException()
        {
            _quizServiceMock.Setup(s => s.DeleteQuizAsync(1, 1)).ThrowsAsync(new KeyNotFoundException());

            var result = await _controller.DeleteQuiz(1, 1);

            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            var response = Assert.IsType<ApiResponse<object>>(notFound.Value);
            Assert.False(response.Success);
        }

        [Fact]
        public async Task DeleteQuiz_ReturnsServerError_OnException()
        {
            _quizServiceMock.Setup(s => s.DeleteQuizAsync(1, 1)).ThrowsAsync(new Exception("fail"));

            var result = await _controller.DeleteQuiz(1, 1);

            var statusResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusResult.StatusCode);
            var response = Assert.IsType<ApiResponse<object>>(statusResult.Value);
            Assert.False(response.Success);
        }

        [Fact]
        public async Task AddQuestionToQuiz_ReturnsCreated_WhenValid()
        {
            var dto = new QuestionAddRequestDTO { Text = "Q", Options = new List<string> { "A" }, CorrectAnswer = "A" };
            var question = new Question { Id = 1, Text = "Q" };
            _quizServiceMock.Setup(s => s.AddQuestionToQuizAsync(1, 1, dto)).ReturnsAsync(question);

            var result = await _controller.AddQuestionToQuiz(1, 1, dto);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var response = Assert.IsType<ApiResponse<Question>>(createdResult.Value);
            Assert.True(response.Success);
            Assert.Equal(question, response.Data);
        }

        [Fact]
        public async Task AddQuestionToQuiz_ReturnsBadRequest_OnArgumentNullException()
        {
            var dto = new QuestionAddRequestDTO();
            _quizServiceMock.Setup(s => s.AddQuestionToQuizAsync(1, 1, dto)).ThrowsAsync(new ArgumentNullException());

            var result = await _controller.AddQuestionToQuiz(1, 1, dto);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<ApiResponse<object>>(badRequest.Value);
            Assert.False(response.Success);
        }

        [Fact]
        public async Task AddQuestionToQuiz_ReturnsBadRequest_OnArgumentException()
        {
            var dto = new QuestionAddRequestDTO();
            _quizServiceMock.Setup(s => s.AddQuestionToQuizAsync(1, 1, dto)).ThrowsAsync(new ArgumentException());

            var result = await _controller.AddQuestionToQuiz(1, 1, dto);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<ApiResponse<object>>(badRequest.Value);
            Assert.False(response.Success);
        }

        [Fact]
        public async Task AddQuestionToQuiz_ReturnsNotFound_OnKeyNotFoundException()
        {
            var dto = new QuestionAddRequestDTO();
            _quizServiceMock.Setup(s => s.AddQuestionToQuizAsync(1, 1, dto)).ThrowsAsync(new KeyNotFoundException());

            var result = await _controller.AddQuestionToQuiz(1, 1, dto);

            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            var response = Assert.IsType<ApiResponse<object>>(notFound.Value);
            Assert.False(response.Success);
        }

        [Fact]
        public async Task AddQuestionToQuiz_ReturnsServerError_OnException()
        {
            var dto = new QuestionAddRequestDTO();
            _quizServiceMock.Setup(s => s.AddQuestionToQuizAsync(1, 1, dto)).ThrowsAsync(new Exception("fail"));

            var result = await _controller.AddQuestionToQuiz(1, 1, dto);

            var statusResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusResult.StatusCode);
            var response = Assert.IsType<ApiResponse<object>>(statusResult.Value);
            Assert.False(response.Success);
        }

        [Fact]
        public async Task UpdateQuestion_ReturnsOk_WhenValid()
        {
            var dto = new QuestionUpdateRequestDTO { Text = "Q", Options = new List<string> { "A" }, CorrectAnswer = "A" };
            var question = new Question { Id = 1, Text = "Q" };
            _quizServiceMock.Setup(s => s.UpdateQuestionAsync(1, 1, dto)).ReturnsAsync(question);

            var result = await _controller.UpdateQuestion(1, 1, dto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<Question>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(question, response.Data);
        }

        [Fact]
        public async Task UpdateQuestion_ReturnsBadRequest_OnArgumentNullException()
        {
            var dto = new QuestionUpdateRequestDTO();
            _quizServiceMock.Setup(s => s.UpdateQuestionAsync(1, 1, dto)).ThrowsAsync(new ArgumentNullException());

            var result = await _controller.UpdateQuestion(1, 1, dto);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<ApiResponse<object>>(badRequest.Value);
            Assert.False(response.Success);
        }

        [Fact]
        public async Task UpdateQuestion_ReturnsBadRequest_OnArgumentException()
        {
            var dto = new QuestionUpdateRequestDTO();
            _quizServiceMock.Setup(s => s.UpdateQuestionAsync(1, 1, dto)).ThrowsAsync(new ArgumentException());

            var result = await _controller.UpdateQuestion(1, 1, dto);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<ApiResponse<object>>(badRequest.Value);
            Assert.False(response.Success);
        }

        [Fact]
        public async Task UpdateQuestion_ReturnsNotFound_OnKeyNotFoundException()
        {
            var dto = new QuestionUpdateRequestDTO();
            _quizServiceMock.Setup(s => s.UpdateQuestionAsync(1, 1, dto)).ThrowsAsync(new KeyNotFoundException());

            var result = await _controller.UpdateQuestion(1, 1, dto);

            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            var response = Assert.IsType<ApiResponse<object>>(notFound.Value);
            Assert.False(response.Success);
        }

        [Fact]
        public async Task UpdateQuestion_ReturnsServerError_OnException()
        {
            var dto = new QuestionUpdateRequestDTO();
            _quizServiceMock.Setup(s => s.UpdateQuestionAsync(1, 1, dto)).ThrowsAsync(new Exception("fail"));

            var result = await _controller.UpdateQuestion(1, 1, dto);

            var statusResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusResult.StatusCode);
            var response = Assert.IsType<ApiResponse<object>>(statusResult.Value);
            Assert.False(response.Success);
        }
    }
}