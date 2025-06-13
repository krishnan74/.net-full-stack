using Microsoft.AspNetCore.Mvc.Testing;
using QuizupAPI.Models;
using QuizupAPI.Models.DTOs.Teacher;
using System.Net;
using System.Text;
using System.Text.Json;

namespace QuizupAPI.Test
{
    public class TeachersControllerIntegrationTests : IntegrationTestBase
    {
        public TeachersControllerIntegrationTests(WebApplicationFactory<Program> factory) : base(factory)
        {
        }

        [Fact]
        public async Task GetAllTeachers_ShouldReturnEmptyList_WhenNoTeachersExist()
        {
            // Act
            var response = await _client.GetAsync("/api/v1/teachers");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            var teachers = JsonSerializer.Deserialize<List<Teacher>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            teachers.Should().NotBeNull();
            teachers.Should().BeEmpty();
        }

        [Fact]
        public async Task CreateTeacher_ShouldReturnCreatedTeacher_WhenValidDataProvided()
        {
            // Arrange
            var teacherDto = new TeacherAddRequestDTO
            {
                Email = "rahulkumar@gmail.com",
                Password = "password123",
                FirstName = "Rahul",
                LastName = "Kumar",
                Subject = "Mathematics"
            };

            var json = JsonSerializer.Serialize(teacherDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/v1/teachers", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var responseContent = await response.Content.ReadAsStringAsync();
            var createdTeacher = JsonSerializer.Deserialize<Teacher>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            createdTeacher.Should().NotBeNull();
            createdTeacher.Email.Should().Be(teacherDto.Email);
            createdTeacher.FirstName.Should().Be(teacherDto.FirstName);
            createdTeacher.LastName.Should().Be(teacherDto.LastName);
            createdTeacher.Subject.Should().Be(teacherDto.Subject);
            createdTeacher.Id.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task CreateTeacher_ShouldReturnBadRequest_WhenInvalidEmailProvided()
        {
            // Arrange
            var teacherDto = new TeacherAddRequestDTO
            {
                Email = "invalid-email",
                Password = "password123",
                FirstName = "Rahul",
                LastName = "Kumar",
                Subject = "Mathematics"
            };

            var json = JsonSerializer.Serialize(teacherDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/v1/teachers", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task CreateTeacher_ShouldReturnBadRequest_WhenRequiredFieldsAreMissing()
        {
            // Arrange
            var teacherDto = new TeacherAddRequestDTO
            {
                Email = "rahulkumar@gmail.com",
            };

            var json = JsonSerializer.Serialize(teacherDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/v1/teachers", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GetTeacherById_ShouldReturnTeacher_WhenTeacherExists()
        {
            // Arrange
            var teacher = new Teacher
            {
                Email = "rahulkumar@gmail.com",
                FirstName = "Rahul",
                LastName = "Kumar",
                Subject = "Mathematics"
            };

            _context.teachers.Add(teacher);
            await _context.SaveChangesAsync();

            // Act
            var response = await _client.GetAsync($"/api/v1/teachers/{teacher.Id}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            var retrievedTeacher = JsonSerializer.Deserialize<Teacher>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            retrievedTeacher.Should().NotBeNull();
            retrievedTeacher.Id.Should().Be(teacher.Id);
            retrievedTeacher.Email.Should().Be(teacher.Email);
        }

        [Fact]
        public async Task GetTeacherById_ShouldReturnNotFound_WhenTeacherDoesNotExist()
        {
            // Act
            var response = await _client.GetAsync("/api/v1/teachers/999");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task UpdateTeacher_ShouldReturnUpdatedTeacher_WhenValidDataProvided()
        {
            // Arrange
            var teacher = new Teacher
            {
                Email = "rahulkumar@gmail.com",
                FirstName = "Rahul",
                LastName = "Kumar",
                Subject = "Mathematics"
            };

            _context.teachers.Add(teacher);
            await _context.SaveChangesAsync();

            var updateDto = new TeacherUpdateRequestDTO
            {
                FirstName = "Dr. Jane",
                LastName = "Doe",
                Subject = "Physics"
            };

            var json = JsonSerializer.Serialize(updateDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync($"/api/v1/teachers/{teacher.Id}", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseContent = await response.Content.ReadAsStringAsync();
            var updatedTeacher = JsonSerializer.Deserialize<Teacher>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            updatedTeacher.Should().NotBeNull();
            updatedTeacher.FirstName.Should().Be(updateDto.FirstName);
            updatedTeacher.LastName.Should().Be(updateDto.LastName);
            updatedTeacher.Subject.Should().Be(updateDto.Subject);
        }

        [Fact]
        public async Task UpdateTeacher_ShouldReturnNotFound_WhenTeacherDoesNotExist()
        {
            // Arrange
            var updateDto = new TeacherUpdateRequestDTO
            {
                FirstName = "Dr. Jane",
                LastName = "Doe",
                Subject = "Physics"
            };

            var json = JsonSerializer.Serialize(updateDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync("/api/v1/teachers/999", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteTeacher_ShouldReturnDeletedTeacher_WhenTeacherExists()
        {
            // Arrange
            var teacher = new Teacher
            {
                Email = "rahulkumar@gmail.com",
                FirstName = "Rahul",
                LastName = "Kumar",
                Subject = "Mathematics"
            };

            _context.teachers.Add(teacher);
            await _context.SaveChangesAsync();

            // Act
            var response = await _client.DeleteAsync($"/api/v1/teachers/{teacher.Id}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            var deletedTeacher = JsonSerializer.Deserialize<Teacher>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            deletedTeacher.Should().NotBeNull();
            deletedTeacher.Id.Should().Be(teacher.Id);

            var getResponse = await _client.GetAsync($"/api/v1/teachers/{teacher.Id}");
            getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteTeacher_ShouldReturnNotFound_WhenTeacherDoesNotExist()
        {
            // Act
            var response = await _client.DeleteAsync("/api/v1/teachers/999");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetAllTeachers_ShouldReturnTeachers_WhenTeachersExist()
        {
            // Arrange
            var teachers = new List<Teacher>
            {
                new Teacher { Email = "teacher1@example.com", FirstName = "Rahul", LastName = "Kumar", Subject = "Mathematics" },
                new Teacher { Email = "teacher2@example.com", FirstName = "Dr. Jane", LastName = "Doe", Subject = "Physics" }
            };

            _context.teachers.AddRange(teachers);
            await _context.SaveChangesAsync();

            // Act
            var response = await _client.GetAsync("/api/v1/teachers");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            var retrievedTeachers = JsonSerializer.Deserialize<List<Teacher>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            retrievedTeachers.Should().NotBeNull();
            retrievedTeachers.Should().HaveCount(2);
            retrievedTeachers.Should().Contain(t => t.Email == "teacher1@example.com");
            retrievedTeachers.Should().Contain(t => t.Email == "teacher2@example.com");
        }

        [Fact]
        public async Task StartQuiz_ShouldReturnQuiz_WhenValidTeacherAndQuizExist()
        {
            // Arrange
            var teacher = new Teacher
            {
                Email = "rahulkumar@gmail.com",
                FirstName = "Rahul",
                LastName = "Kumar",
                Subject = "Mathematics"
            };

            var quiz = new Quiz
            {
                Title = "Math Quiz",
                Description = "Basic math questions",
                TeacherId = 1, 
                IsActive = false
            };

            _context.teachers.Add(teacher);
            await _context.SaveChangesAsync();

            quiz.TeacherId = teacher.Id;
            _context.quizzes.Add(quiz);
            await _context.SaveChangesAsync();

            // Act
            var response = await _client.PostAsync($"/api/v1/teachers/{teacher.Id}/quizzes/{quiz.Id}/start", null);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            var updatedQuiz = JsonSerializer.Deserialize<Quiz>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            updatedQuiz.Should().NotBeNull();
            updatedQuiz.IsActive.Should().BeTrue();
        }

        [Fact]
        public async Task EndQuiz_ShouldReturnQuiz_WhenValidTeacherAndQuizExist()
        {
            // Arrange
            var teacher = new Teacher
            {
                Email = "rahulkumar@gmail.com",
                FirstName = "Rahul",
                LastName = "Kumar",
                Subject = "Mathematics"
            };

            var quiz = new Quiz
            {
                Title = "Math Quiz",
                Description = "Basic math questions",
                TeacherId = 1, 
                IsActive = true
            };

            _context.teachers.Add(teacher);
            await _context.SaveChangesAsync();

            quiz.TeacherId = teacher.Id;
            _context.quizzes.Add(quiz);
            await _context.SaveChangesAsync();

            // Act
            var response = await _client.PostAsync($"/api/v1/teachers/{teacher.Id}/quizzes/{quiz.Id}/end", null);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            var updatedQuiz = JsonSerializer.Deserialize<Quiz>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            updatedQuiz.Should().NotBeNull();
            updatedQuiz.IsActive.Should().BeFalse();
        }
    }
} 