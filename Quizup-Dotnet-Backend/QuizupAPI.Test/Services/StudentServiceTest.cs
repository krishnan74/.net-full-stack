using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Moq;
using QuizupAPI.Services;
using QuizupAPI.Interfaces;
using QuizupAPI.Models;
using QuizupAPI.Models.DTOs.Student;
using QuizupAPI.Models.DTOs.QuizSubmission;
using QuizupAPI.Repositories;
using QuizupAPI.Contexts;
using Microsoft.EntityFrameworkCore;

namespace QuizupAPI.Test.Services
{
    public class StudentServiceTest
    {
        private readonly Mock<IRepository<long, Student>> _studentRepoMock = new();
        private readonly Mock<IRepository<long, Quiz>> _quizRepoMock = new();
        private readonly Mock<IEncryptionService> _encryptionServiceMock = new();
        private readonly Mock<IRepository<string, User>> _userRepoMock = new();
        private readonly Mock<IRepository<long, QuizSubmission>> _quizSubmissionRepoMock = new();
        private readonly Mock<IRepository<long, Answer>> _answerRepoMock = new();
        private readonly Mock<QuizContext> _contextMock;

        private readonly StudentService _service;

        public StudentServiceTest()
        {
            var options = new DbContextOptionsBuilder<QuizContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _contextMock = new Mock<QuizContext>(options);

            _service = new StudentService(
                _studentRepoMock.Object,
                _quizRepoMock.Object,
                _encryptionServiceMock.Object,
                _userRepoMock.Object,
                _quizSubmissionRepoMock.Object,
                _answerRepoMock.Object,
                _contextMock.Object
            );
        }

        [Fact]
        public async Task AddStudentAsync_ShouldThrow_WhenDtoIsNull()
        {
            await Assert.ThrowsAsync<Exception>(() => _service.AddStudentAsync(null));
        }

        [Fact]
        public async Task AddStudentAsync_ShouldThrow_WhenRequiredFieldsMissing()
        {
            var dto = new StudentAddRequestDTO { FirstName = "", LastName = "", Email = "", Password = "" };
            await Assert.ThrowsAsync<Exception>(() => _service.AddStudentAsync(dto));
        }

        [Fact]
        public async Task AddStudentAsync_ShouldThrow_WhenEmailInvalid()
        {
            var dto = new StudentAddRequestDTO { FirstName = "A", LastName = "B", Email = "bademail", Password = "pass" };
            await Assert.ThrowsAsync<Exception>(() => _service.AddStudentAsync(dto));
        }

        [Fact]
        public async Task AddStudentAsync_ShouldThrow_WhenStudentExists()
        {
            var dto = new StudentAddRequestDTO { FirstName = "A", LastName = "B", Email = "test@email.com", Password = "pass" };
            _userRepoMock.Setup(r => r.Add(It.IsAny<User>())).ReturnsAsync(new User());
            _studentRepoMock.Setup(r => r.GetAll()).ReturnsAsync(new List<Student> { new Student { Email = "test@email.com" } });

            await Assert.ThrowsAsync<Exception>(() => _service.AddStudentAsync(dto));
        }

        [Fact]
        public async Task GetStudentByIdAsync_ShouldReturnStudent()
        {
            var student = new Student { Id = 1, FirstName = "A" };
            _studentRepoMock.Setup(r => r.Get(1)).ReturnsAsync(student);

            var result = await _service.GetStudentByIdAsync(1);

            Assert.Equal(student, result);
        }

        [Fact]
        public async Task GetStudentByIdAsync_ShouldThrow_WhenNotFound()
        {
            _studentRepoMock.Setup(r => r.Get(It.IsAny<long>())).ThrowsAsync(new KeyNotFoundException("not found"));
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.GetStudentByIdAsync(99));
        }

        [Fact]
        public async Task GetAllStudentsAsync_ShouldReturnStudents()
        {
            var students = new List<Student> { new Student { Id = 1 }, new Student { Id = 2 } };
            _studentRepoMock.Setup(r => r.GetAll()).ReturnsAsync(students);

            var result = await _service.GetAllStudentsAsync();

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task UpdateStudentAsync_ShouldThrow_WhenDtoIsNull()
        {
            await Assert.ThrowsAsync<Exception>(() => _service.UpdateStudentAsync(1, null));
        }

        [Fact]
        public async Task UpdateStudentAsync_ShouldThrow_WhenRequiredFieldsMissing()
        {
            var dto = new StudentUpdateRequestDTO { FirstName = "", LastName = "", Class = "" };
            await Assert.ThrowsAsync<Exception>(() => _service.UpdateStudentAsync(1, dto));
        }

        [Fact]
        public async Task UpdateStudentAsync_ShouldThrow_WhenNotFound()
        {
            _studentRepoMock.Setup(r => r.Get(It.IsAny<long>())).ThrowsAsync(new KeyNotFoundException("not found"));
            var dto = new StudentUpdateRequestDTO { FirstName = "A", LastName = "B", Class = "10" };
            await Assert.ThrowsAsync<Exception>(() => _service.UpdateStudentAsync(1, dto));
        }

        [Fact]
        public async Task DeleteStudentAsync_ShouldThrow_WhenNotFound()
        {
            _studentRepoMock.Setup(r => r.Get(It.IsAny<long>())).ThrowsAsync(new KeyNotFoundException("not found"));
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.DeleteStudentAsync(1));
        }

        [Fact]
        public async Task GetStudentsPaginationAsync_ShouldThrow_WhenInvalidPage()
        {
            await Assert.ThrowsAsync<Exception>(() => _service.GetStudentsPaginationAsync(0, 10));
            await Assert.ThrowsAsync<Exception>(() => _service.GetStudentsPaginationAsync(1, 0));
        }

        [Fact]
        public async Task GetStudentsPaginationAsync_ShouldReturnPaged()
        {
            var students = Enumerable.Range(1, 20).Select(i => new Student { Id = i }).ToList();
            _studentRepoMock.Setup(r => r.GetAll()).ReturnsAsync(students);

            var result = await _service.GetStudentsPaginationAsync(2, 5);

            Assert.Equal(5, result.Count());
            Assert.Equal(6, result.First().Id);
        }
    }
}