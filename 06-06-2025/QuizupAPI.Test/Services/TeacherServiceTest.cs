using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Moq;
using QuizupAPI.Services;
using QuizupAPI.Interfaces;
using QuizupAPI.Models;
using QuizupAPI.Models.DTOs.Teacher;
using QuizupAPI.Repositories;
using Microsoft.AspNetCore.SignalR;
using QuizupAPI.Hubs;
using QuizupAPI.Contexts;
using Microsoft.EntityFrameworkCore;

namespace QuizupAPI.Test.Services
{
    public class TeacherServiceTest
    {
        private readonly Mock<IRepository<long, Teacher>> _teacherRepoMock = new();
        private readonly Mock<IRepository<long, Quiz>> _quizRepoMock = new();
        private readonly Mock<IEncryptionService> _encryptionServiceMock = new();
        private readonly Mock<IRepository<string, User>> _userRepoMock = new();
        private readonly Mock<IHubContext<QuizNotificationHub>> _hubContextMock = new();
        private readonly Mock<QuizContext> _contextMock;

        private readonly TeacherService _service;

        public TeacherServiceTest()
        {
            var options = new DbContextOptionsBuilder<QuizContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _contextMock = new Mock<QuizContext>(options);

            _service = new TeacherService(
                _teacherRepoMock.Object,
                _quizRepoMock.Object,
                _hubContextMock.Object,
                _encryptionServiceMock.Object,
                _userRepoMock.Object,
                _contextMock.Object
            );
        }

        [Fact]
        public async Task AddTeacherAsync_ShouldThrow_WhenDtoIsNull()
        {
            await Assert.ThrowsAsync<Exception>(() => _service.AddTeacherAsync(null));
        }

        [Fact]
        public async Task AddTeacherAsync_ShouldThrow_WhenRequiredFieldsMissing()
        {
            var dto = new TeacherAddRequestDTO { FirstName = "", LastName = "", Email = "", Password = "" };
            await Assert.ThrowsAsync<Exception>(() => _service.AddTeacherAsync(dto));
        }

        [Fact]
        public async Task AddTeacherAsync_ShouldThrow_WhenEmailInvalid()
        {
            var dto = new TeacherAddRequestDTO { FirstName = "A", LastName = "B", Email = "bademail", Password = "pass" };
            await Assert.ThrowsAsync<Exception>(() => _service.AddTeacherAsync(dto));
        }

        [Fact]
        public async Task AddTeacherAsync_ShouldThrow_WhenTeacherExists()
        {
            var dto = new TeacherAddRequestDTO { FirstName = "A", LastName = "B", Email = "test@email.com", Password = "pass" };
            _userRepoMock.Setup(r => r.Add(It.IsAny<User>())).ReturnsAsync(new User());
            _teacherRepoMock.Setup(r => r.GetAll()).ReturnsAsync(new List<Teacher> { new Teacher { Email = "test@email.com" } });

            await Assert.ThrowsAsync<Exception>(() => _service.AddTeacherAsync(dto));
        }

        [Fact]
        public async Task GetTeacherByIdAsync_ShouldReturnTeacher()
        {
            var teacher = new Teacher { Id = 1, FirstName = "A" };
            _teacherRepoMock.Setup(r => r.Get(1)).ReturnsAsync(teacher);

            var result = await _service.GetTeacherByIdAsync(1);

            Assert.Equal(teacher, result);
        }

        [Fact]
        public async Task GetTeacherByIdAsync_ShouldThrow_WhenNotFound()
        {
            _teacherRepoMock.Setup(r => r.Get(It.IsAny<long>())).ThrowsAsync(new KeyNotFoundException("not found"));
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.GetTeacherByIdAsync(99));
        }

        [Fact]
        public async Task GetAllTeachersAsync_ShouldReturnTeachers()
        {
            var teachers = new List<Teacher> { new Teacher { Id = 1 }, new Teacher { Id = 2 } };
            _teacherRepoMock.Setup(r => r.GetAll()).ReturnsAsync(teachers);

            var result = await _service.GetAllTeachersAsync();

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task UpdateTeacherAsync_ShouldThrow_WhenDtoIsNull()
        {
            await Assert.ThrowsAsync<Exception>(() => _service.UpdateTeacherAsync(1, null));
        }

        [Fact]
        public async Task UpdateTeacherAsync_ShouldThrow_WhenRequiredFieldsMissing()
        {
            var dto = new TeacherUpdateRequestDTO { FirstName = "", LastName = "", Subject = "" };
            await Assert.ThrowsAsync<Exception>(() => _service.UpdateTeacherAsync(1, dto));
        }

        [Fact]
        public async Task UpdateTeacherAsync_ShouldThrow_WhenNotFound()
        {
            _teacherRepoMock.Setup(r => r.Get(It.IsAny<long>())).ThrowsAsync(new KeyNotFoundException("not found"));
            var dto = new TeacherUpdateRequestDTO { FirstName = "A", LastName = "B", Subject = "Math" };
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.UpdateTeacherAsync(1, dto));
        }

        [Fact]
        public async Task DeleteTeacherAsync_ShouldThrow_WhenNotFound()
        {
            _teacherRepoMock.Setup(r => r.Get(It.IsAny<long>())).ThrowsAsync(new KeyNotFoundException("not found"));
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.DeleteTeacherAsync(1));
        }

        [Fact]
        public async Task GetTeachersPaginationAsync_ShouldThrow_WhenInvalidPage()
        {
            await Assert.ThrowsAsync<Exception>(() => _service.GetTeachersPaginationAsync(0, 10));
            await Assert.ThrowsAsync<Exception>(() => _service.GetTeachersPaginationAsync(1, 0));
        }

        [Fact]
        public async Task GetTeachersPaginationAsync_ShouldReturnPaged()
        {
            var teachers = Enumerable.Range(1, 20).Select(i => new Teacher { Id = i }).ToList();
            _teacherRepoMock.Setup(r => r.GetAll()).ReturnsAsync(teachers);

            var result = await _service.GetTeachersPaginationAsync(2, 5);

            Assert.Equal(5, result.Count());
            Assert.Equal(6, result.First().Id);
        }
    }
}