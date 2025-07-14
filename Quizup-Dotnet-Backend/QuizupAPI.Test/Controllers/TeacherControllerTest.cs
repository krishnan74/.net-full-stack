using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using QuizupAPI.Controllers;
using QuizupAPI.Interfaces;
using QuizupAPI.Models;
using QuizupAPI.Models.DTOs.Teacher;
using QuizupAPI.Models.DTOs.Response;

namespace QuizupAPI.Test.Controllers
{
    public class TeacherControllerTest
    {
        private readonly Mock<ITeacherService> _teacherServiceMock = new();
        private readonly TeachersController _controller;

        public TeacherControllerTest()
        {
            _controller = new TeachersController(_teacherServiceMock.Object);
        }

        [Fact]
        public async Task GetAllTeachers_ReturnsOk_WithApiResponse()
        {
            var teachers = new List<Teacher> { new Teacher { Id = 1 }, new Teacher { Id = 2 } };
            _teacherServiceMock.Setup(s => s.GetAllTeachersAsync()).ReturnsAsync(teachers);

            var result = await _controller.GetAllTeachers();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<IEnumerable<Teacher>>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(teachers, response.Data);
        }

        [Fact]
        public async Task GetTeacherById_ReturnsOk_WhenFound()
        {
            var teacher = new Teacher { Id = 1 };
            _teacherServiceMock.Setup(s => s.GetTeacherByIdAsync(1)).ReturnsAsync(teacher);

            var result = await _controller.GetTeacherById(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<Teacher>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(teacher, response.Data);
        }

        [Fact]
        public async Task GetTeacherById_ReturnsNotFound_WhenNotFound()
        {
            _teacherServiceMock.Setup(s => s.GetTeacherByIdAsync(1)).ThrowsAsync(new KeyNotFoundException("not found"));

            var result = await _controller.GetTeacherById(1);

            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            var response = Assert.IsType<ApiResponse<object>>(notFound.Value);
            Assert.False(response.Success);
        }

        [Fact]
        public async Task GetTeachersPagination_ReturnsOk_WithApiResponse()
        {
            var teachers = new List<Teacher> { new Teacher { Id = 1 }, new Teacher { Id = 2 } };
            _teacherServiceMock.Setup(s => s.GetTeachersPaginationAsync(1, 10)).ReturnsAsync(teachers);
            _teacherServiceMock.Setup(s => s.GetAllTeachersAsync()).ReturnsAsync(teachers);

            var result = await _controller.GetTeachersPagination(1, 10);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<PaginatedResponse<IEnumerable<Teacher>>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(teachers, response.Data);
        }

        [Fact]
        public async Task GetTeachersPagination_ReturnsBadRequest_WhenInvalidPage()
        {
            var result = await _controller.GetTeachersPagination(0, 0);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<ApiResponse<object>>(badRequest.Value);
            Assert.False(response.Success);
        }

        [Fact]
        public async Task CreateTeacher_ReturnsCreated_WhenValid()
        {
            var dto = new TeacherAddRequestDTO { FirstName = "A", LastName = "B", Email = "a@b.com", Password = "pass" };
            var teacher = new Teacher { Id = 1, FirstName = "A" };
            _teacherServiceMock.Setup(s => s.AddTeacherAsync(dto)).ReturnsAsync(teacher);

            var result = await _controller.CreateTeacher(dto);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var response = Assert.IsType<ApiResponse<Teacher>>(createdResult.Value);
            Assert.True(response.Success);
            Assert.Equal(teacher, response.Data);
        }

        [Fact]
        public async Task CreateTeacher_ReturnsBadRequest_OnArgumentNullException()
        {
            var dto = new TeacherAddRequestDTO();
            _teacherServiceMock.Setup(s => s.AddTeacherAsync(dto)).ThrowsAsync(new ArgumentNullException());

            var result = await _controller.CreateTeacher(dto);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<ApiResponse<object>>(badRequest.Value);
            Assert.False(response.Success);
        }

        [Fact]
        public async Task CreateTeacher_ReturnsBadRequest_OnArgumentException()
        {
            var dto = new TeacherAddRequestDTO();
            _teacherServiceMock.Setup(s => s.AddTeacherAsync(dto)).ThrowsAsync(new ArgumentException());

            var result = await _controller.CreateTeacher(dto);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<ApiResponse<object>>(badRequest.Value);
            Assert.False(response.Success);
        }

        [Fact]
        public async Task UpdateTeacher_ReturnsOk_WhenValid()
        {
            var dto = new TeacherUpdateRequestDTO { FirstName = "A", LastName = "B", Subject = "Math" };
            var teacher = new Teacher { Id = 1, FirstName = "A" };
            _teacherServiceMock.Setup(s => s.UpdateTeacherAsync(1, dto)).ReturnsAsync(teacher);

            var result = await _controller.UpdateTeacher(1, dto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<Teacher>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(teacher, response.Data);
        }

        [Fact]
        public async Task UpdateTeacher_ReturnsBadRequest_OnArgumentNullException()
        {
            var dto = new TeacherUpdateRequestDTO();
            _teacherServiceMock.Setup(s => s.UpdateTeacherAsync(1, dto)).ThrowsAsync(new ArgumentNullException());

            var result = await _controller.UpdateTeacher(1, dto);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<ApiResponse<object>>(badRequest.Value);
            Assert.False(response.Success);
        }

        [Fact]
        public async Task UpdateTeacher_ReturnsBadRequest_OnArgumentException()
        {
            var dto = new TeacherUpdateRequestDTO();
            _teacherServiceMock.Setup(s => s.UpdateTeacherAsync(1, dto)).ThrowsAsync(new ArgumentException());

            var result = await _controller.UpdateTeacher(1, dto);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<ApiResponse<object>>(badRequest.Value);
            Assert.False(response.Success);
        }

        [Fact]
        public async Task UpdateTeacher_ReturnsNotFound_WhenNotFound()
        {
            var dto = new TeacherUpdateRequestDTO();
            _teacherServiceMock.Setup(s => s.UpdateTeacherAsync(1, dto)).ThrowsAsync(new KeyNotFoundException());

            var result = await _controller.UpdateTeacher(1, dto);

            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            var response = Assert.IsType<ApiResponse<object>>(notFound.Value);
            Assert.False(response.Success);
        }

        [Fact]
        public async Task DeleteTeacher_ReturnsOk_WhenValid()
        {
            var teacher = new Teacher { Id = 1 };
            _teacherServiceMock.Setup(s => s.DeleteTeacherAsync(1)).ReturnsAsync(teacher);

            var result = await _controller.DeleteTeacher(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<Teacher>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(teacher, response.Data);
        }

        [Fact]
        public async Task DeleteTeacher_ReturnsNotFound_WhenNotFound()
        {
            _teacherServiceMock.Setup(s => s.DeleteTeacherAsync(1)).ThrowsAsync(new KeyNotFoundException());

            var result = await _controller.DeleteTeacher(1);

            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            var response = Assert.IsType<ApiResponse<object>>(notFound.Value);
            Assert.False(response.Success);
        }

        [Fact]
        public async Task StartQuiz_ReturnsOk_WhenValid()
        {
            var quiz = new Quiz { Id = 1 };
            _teacherServiceMock.Setup(s => s.StartQuizAsync(1, 2)).ReturnsAsync(quiz);

            var result = await _controller.StartQuiz(1, 2);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<Quiz>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(quiz, response.Data);
        }

        [Fact]
        public async Task StartQuiz_ReturnsNotFound_WhenNotFound()
        {
            _teacherServiceMock.Setup(s => s.StartQuizAsync(1, 2)).ThrowsAsync(new KeyNotFoundException());

            var result = await _controller.StartQuiz(1, 2);

            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            var response = Assert.IsType<ApiResponse<object>>(notFound.Value);
            Assert.False(response.Success);
        }

        [Fact]
        public async Task EndQuiz_ReturnsOk_WhenValid()
        {
            var quiz = new Quiz { Id = 1 };
            _teacherServiceMock.Setup(s => s.EndQuizAsync(1, 2)).ReturnsAsync(quiz);

            var result = await _controller.EndQuiz(1, 2);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<Quiz>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(quiz, response.Data);
        }

        [Fact]
        public async Task EndQuiz_ReturnsNotFound_WhenNotFound()
        {
            _teacherServiceMock.Setup(s => s.EndQuizAsync(1, 2)).ThrowsAsync(new KeyNotFoundException());

            var result = await _controller.EndQuiz(1, 2);

            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            var response = Assert.IsType<ApiResponse<object>>(notFound.Value);
            Assert.False(response.Success);
        }

        [Fact]
        public async Task GetTeacherQuizSummary_ReturnsOk_WhenValid()
        {
            var summary = new TeacherSummaryDTO();
            _teacherServiceMock.Setup(s => s.GetTeacherQuizSummaryAsync(1, null, null)).ReturnsAsync(summary);

            var result = await _controller.GetTeacherQuizSummary(1, null, null);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<TeacherSummaryDTO>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(summary, response.Data);
        }

        [Fact]
        public async Task GetTeacherQuizSummary_ReturnsNotFound_WhenNotFound()
        {
            _teacherServiceMock.Setup(s => s.GetTeacherQuizSummaryAsync(1, null, null)).ThrowsAsync(new KeyNotFoundException());

            var result = await _controller.GetTeacherQuizSummary(1, null, null);

            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            var response = Assert.IsType<ApiResponse<object>>(notFound.Value);
            Assert.False(response.Success);
        }
    }
}