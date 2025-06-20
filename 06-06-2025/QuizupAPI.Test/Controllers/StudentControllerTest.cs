using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using QuizupAPI.Controllers;
using QuizupAPI.Interfaces;
using QuizupAPI.Models;
using QuizupAPI.Models.DTOs.Student;
using QuizupAPI.Models.DTOs.QuizSubmission;
using QuizupAPI.Models.DTOs.Response;

namespace QuizupAPI.Test.Controllers
{
    public class StudentControllerTest
    {
        private readonly Mock<IStudentService> _studentServiceMock = new();
        private readonly StudentsController _controller;

        public StudentControllerTest()
        {
            _controller = new StudentsController(_studentServiceMock.Object);
        }

        [Fact]
        public async Task GetAllStudents_ReturnsOk_WithApiResponse()
        {
            var students = new List<Student> { new Student { Id = 1 }, new Student { Id = 2 } };
            _studentServiceMock.Setup(s => s.GetAllStudentsAsync()).ReturnsAsync(students);

            var result = await _controller.GetAllStudents();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<IEnumerable<Student>>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(students, response.Data);
        }

        [Fact]
        public async Task GetStudentById_ReturnsOk_WhenFound()
        {
            var student = new Student { Id = 1 };
            _studentServiceMock.Setup(s => s.GetStudentByIdAsync(1)).ReturnsAsync(student);

            var result = await _controller.GetStudentById(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<Student>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(student, response.Data);
        }

        [Fact]
        public async Task GetStudentById_ReturnsNotFound_WhenNotFound()
        {
            _studentServiceMock.Setup(s => s.GetStudentByIdAsync(1)).ThrowsAsync(new KeyNotFoundException("not found"));

            var result = await _controller.GetStudentById(1);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var response = Assert.IsType<ApiResponse<object>>(notFoundResult.Value);
            Assert.False(response.Success);
        }

        [Fact]
        public async Task GetStudentsPagination_ReturnsOk_WithApiResponse()
        {
            var students = new List<Student> { new Student { Id = 1 }, new Student { Id = 2 } };
            _studentServiceMock.Setup(s => s.GetStudentsPaginationAsync(1, 10)).ReturnsAsync(students);
            _studentServiceMock.Setup(s => s.GetAllStudentsAsync()).ReturnsAsync(students);

            var result = await _controller.GetStudentsPagination(1, 10);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<PaginatedResponse<IEnumerable<Student>>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(students, response.Data);
        }

        [Fact]
        public async Task GetStudentsPagination_ReturnsBadRequest_WhenInvalidPage()
        {
            var result = await _controller.GetStudentsPagination(0, 0);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<ApiResponse<object>>(badRequest.Value);
            Assert.False(response.Success);
        }

        [Fact]
        public async Task CreateStudent_ReturnsCreated_WhenValid()
        {
            var dto = new StudentAddRequestDTO { FirstName = "A", LastName = "B", Email = "a@b.com", Password = "pass" };
            var student = new Student { Id = 1, FirstName = "A" };
            _studentServiceMock.Setup(s => s.AddStudentAsync(dto)).ReturnsAsync(student);

            var result = await _controller.CreateStudent(dto);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var response = Assert.IsType<ApiResponse<Student>>(createdResult.Value);
            Assert.True(response.Success);
            Assert.Equal(student, response.Data);
        }

        [Fact]
        public async Task CreateStudent_ReturnsBadRequest_OnArgumentNullException()
        {
            var dto = new StudentAddRequestDTO();
            _studentServiceMock.Setup(s => s.AddStudentAsync(dto)).ThrowsAsync(new ArgumentNullException());

            var result = await _controller.CreateStudent(dto);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<ApiResponse<object>>(badRequest.Value);
            Assert.False(response.Success);
        }

        [Fact]
        public async Task CreateStudent_ReturnsConflict_OnInvalidOperationException()
        {
            var dto = new StudentAddRequestDTO();
            _studentServiceMock.Setup(s => s.AddStudentAsync(dto)).ThrowsAsync(new InvalidOperationException());

            var result = await _controller.CreateStudent(dto);

            var conflict = Assert.IsType<ConflictObjectResult>(result);
            var response = Assert.IsType<ApiResponse<object>>(conflict.Value);
            Assert.False(response.Success);
        }

        [Fact]
        public async Task UpdateStudent_ReturnsOk_WhenValid()
        {
            var dto = new StudentUpdateRequestDTO { FirstName = "A", LastName = "B", Class = "10" };
            var student = new Student { Id = 1, FirstName = "A" };
            _studentServiceMock.Setup(s => s.UpdateStudentAsync(1, dto)).ReturnsAsync(student);

            var result = await _controller.UpdateStudent(1, dto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<Student>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(student, response.Data);
        }

        [Fact]
        public async Task UpdateStudent_ReturnsNotFound_WhenNotFound()
        {
            var dto = new StudentUpdateRequestDTO();
            _studentServiceMock.Setup(s => s.UpdateStudentAsync(1, dto)).ThrowsAsync(new KeyNotFoundException());

            var result = await _controller.UpdateStudent(1, dto);

            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            var response = Assert.IsType<ApiResponse<object>>(notFound.Value);
            Assert.False(response.Success);
        }

        [Fact]
        public async Task DeleteStudent_ReturnsOk_WhenValid()
        {
            var student = new Student { Id = 1 };
            _studentServiceMock.Setup(s => s.DeleteStudentAsync(1)).ReturnsAsync(student);

            var result = await _controller.DeleteStudent(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<Student>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(student, response.Data);
        }

        [Fact]
        public async Task DeleteStudent_ReturnsNotFound_WhenNotFound()
        {
            _studentServiceMock.Setup(s => s.DeleteStudentAsync(1)).ThrowsAsync(new KeyNotFoundException());

            var result = await _controller.DeleteStudent(1);

            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            var response = Assert.IsType<ApiResponse<object>>(notFound.Value);
            Assert.False(response.Success);
        }

        [Fact]
        public async Task GetStudentSubmissions_ReturnsOk_WhenFound()
        {
            var submissions = new List<QuizSubmission> { new QuizSubmission { Id = 1 } };
            _studentServiceMock.Setup(s => s.GetSubmissionsByStudentIdAsync(1)).ReturnsAsync(submissions);

            var result = await _controller.GetStudentSubmissions(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<IEnumerable<QuizSubmission>>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(submissions, response.Data);
        }

        [Fact]
        public async Task GetStudentSubmissions_ReturnsNotFound_WhenNotFound()
        {
            _studentServiceMock.Setup(s => s.GetSubmissionsByStudentIdAsync(1)).ThrowsAsync(new KeyNotFoundException());

            var result = await _controller.GetStudentSubmissions(1);

            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            var response = Assert.IsType<ApiResponse<object>>(notFound.Value);
            Assert.False(response.Success);
        }

        [Fact]
        public async Task StartQuiz_ReturnsOk_WhenValid()
        {
            var submission = new QuizSubmission { Id = 1 };
            _studentServiceMock.Setup(s => s.StartQuizAsync(1, 2)).ReturnsAsync(submission);

            var result = await _controller.StartQuiz(1, 2);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<QuizSubmission>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(submission, response.Data);
        }

        [Fact]
        public async Task StartQuiz_ReturnsNotFound_WhenNotFound()
        {
            _studentServiceMock.Setup(s => s.StartQuizAsync(1, 2)).ThrowsAsync(new KeyNotFoundException());

            var result = await _controller.StartQuiz(1, 2);

            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            var response = Assert.IsType<ApiResponse<object>>(notFound.Value);
            Assert.False(response.Success);
        }

        [Fact]
        public async Task SubmitQuiz_ReturnsOk_WhenValid()
        {
            var submission = new QuizSubmission { Id = 1 };
            var dto = new QuizSubmissionDTO();
            _studentServiceMock.Setup(s => s.SubmitQuizAsync(1, 2, dto)).ReturnsAsync(submission);

            var result = await _controller.SubmitQuiz(1, 2, dto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<QuizSubmission>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(submission, response.Data);
        }

        [Fact]
        public async Task SubmitQuiz_ReturnsBadRequest_OnArgumentNullException()
        {
            var dto = new QuizSubmissionDTO();
            _studentServiceMock.Setup(s => s.SubmitQuizAsync(1, 2, dto)).ThrowsAsync(new ArgumentNullException());

            var result = await _controller.SubmitQuiz(1, 2, dto);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<ApiResponse<object>>(badRequest.Value);
            Assert.False(response.Success);
        }

        [Fact]
        public async Task SubmitQuiz_ReturnsNotFound_WhenNotFound()
        {
            var dto = new QuizSubmissionDTO();
            _studentServiceMock.Setup(s => s.SubmitQuizAsync(1, 2, dto)).ThrowsAsync(new KeyNotFoundException());

            var result = await _controller.SubmitQuiz(1, 2, dto);

            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            var response = Assert.IsType<ApiResponse<object>>(notFound.Value);
            Assert.False(response.Success);
        }

        [Fact]
        public async Task SaveQuizAnswers_ReturnsOk_WhenValid()
        {
            var submission = new QuizSubmission { Id = 1 };
            var dto = new QuizSubmissionDTO();
            _studentServiceMock.Setup(s => s.SaveAnswersAsync(1, 2, dto)).ReturnsAsync(submission);

            var result = await _controller.SaveQuizAnswers(1, 2, dto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<QuizSubmission>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(submission, response.Data);
        }

        [Fact]
        public async Task SaveQuizAnswers_ReturnsBadRequest_OnArgumentNullException()
        {
            var dto = new QuizSubmissionDTO();
            _studentServiceMock.Setup(s => s.SaveAnswersAsync(1, 2, dto)).ThrowsAsync(new ArgumentNullException());

            var result = await _controller.SaveQuizAnswers(1, 2, dto);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<ApiResponse<object>>(badRequest.Value);
            Assert.False(response.Success);
        }

        [Fact]
        public async Task SaveQuizAnswers_ReturnsNotFound_WhenNotFound()
        {
            var dto = new QuizSubmissionDTO();
            _studentServiceMock.Setup(s => s.SaveAnswersAsync(1, 2, dto)).ThrowsAsync(new KeyNotFoundException());

            var result = await _controller.SaveQuizAnswers(1, 2, dto);

            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            var response = Assert.IsType<ApiResponse<object>>(notFound.Value);
            Assert.False(response.Success);
        }

        [Fact]
        public async Task GetStudentQuizSummary_ReturnsOk_WhenValid()
        {
            var summary = new StudentSummaryDTO();
            _studentServiceMock.Setup(s => s.GetStudentQuizSummaryAsync(1, null, null)).ReturnsAsync(summary);

            var result = await _controller.GetStudentQuizSummary(1, null, null);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponse<StudentSummaryDTO>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(summary, response.Data);
        }

        [Fact]
        public async Task GetStudentQuizSummary_ReturnsNotFound_WhenNotFound()
        {
            _studentServiceMock.Setup(s => s.GetStudentQuizSummaryAsync(1, null, null)).ThrowsAsync(new KeyNotFoundException());

            var result = await _controller.GetStudentQuizSummary(1, null, null);

            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            var response = Assert.IsType<ApiResponse<object>>(notFound.Value);
            Assert.False(response.Success);
        }
    }
}