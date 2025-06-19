using Microsoft.AspNetCore.Mvc.Testing;
using QuizupAPI.Models;
using QuizupAPI.Models.DTOs.Teacher;
using QuizupAPI.Models.DTOs.Response;
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
            var apiResponse = JsonSerializer.Deserialize<ApiResponse<IEnumerable<Teacher>>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            apiResponse.Should().NotBeNull();
            apiResponse.Success.Should().BeTrue();
            apiResponse.Data.Should().NotBeNull();
            apiResponse.Data.Should().BeEmpty();
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
            var apiResponse = JsonSerializer.Deserialize<ApiResponse<Teacher>>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            apiResponse.Should().NotBeNull();
            apiResponse.Success.Should().BeTrue();
            apiResponse.Data.Should().NotBeNull();
            apiResponse.Data.Email.Should().Be(teacherDto.Email);
            apiResponse.Data.FirstName.Should().Be(teacherDto.FirstName);
            apiResponse.Data.LastName.Should().Be(teacherDto.LastName);
            apiResponse.Data.Subject.Should().Be(teacherDto.Subject);
            apiResponse.Data.Id.Should().BeGreaterThan(0);
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
            var apiResponse = JsonSerializer.Deserialize<ApiResponse<Teacher>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            apiResponse.Should().NotBeNull();
            apiResponse.Success.Should().BeTrue();
            apiResponse.Data.Should().NotBeNull();
            apiResponse.Data.Id.Should().Be(teacher.Id);
            apiResponse.Data.Email.Should().Be(teacher.Email);
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
            var apiResponse = JsonSerializer.Deserialize<ApiResponse<Teacher>>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            apiResponse.Should().NotBeNull();
            apiResponse.Success.Should().BeTrue();
            apiResponse.Data.Should().NotBeNull();
            apiResponse.Data.FirstName.Should().Be(updateDto.FirstName);
            apiResponse.Data.LastName.Should().Be(updateDto.LastName);
            apiResponse.Data.Subject.Should().Be(updateDto.Subject);
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
            var apiResponse = JsonSerializer.Deserialize<ApiResponse<Teacher>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            apiResponse.Should().NotBeNull();
            apiResponse.Success.Should().BeTrue();
            apiResponse.Data.Should().NotBeNull();
            apiResponse.Data.Id.Should().Be(teacher.Id);

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
                new Teacher { Email = "teacher1@gmail.com", FirstName = "John", LastName = "Doe", Subject = "Mathematics" },
                new Teacher { Email = "teacher2@gmail.com", FirstName = "Jane", LastName = "Smith", Subject = "Physics" }
            };

            _context.teachers.AddRange(teachers);
            await _context.SaveChangesAsync();

            // Act
            var response = await _client.GetAsync("/api/v1/teachers");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<ApiResponse<IEnumerable<Teacher>>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            apiResponse.Should().NotBeNull();
            apiResponse.Success.Should().BeTrue();
            apiResponse.Data.Should().NotBeNull();
            apiResponse.Data.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetTeachersPagination_ShouldReturnPaginatedTeachers_WhenValidParametersProvided()
        {
            // Arrange
            var teachers = new List<Teacher>
            {
                new Teacher { Email = "teacher1@gmail.com", FirstName = "John", LastName = "Doe", Subject = "Mathematics" },
                new Teacher { Email = "teacher2@gmail.com", FirstName = "Jane", LastName = "Smith", Subject = "Physics" },
                new Teacher { Email = "teacher3@gmail.com", FirstName = "Bob", LastName = "Johnson", Subject = "Chemistry" }
            };

            _context.teachers.AddRange(teachers);
            await _context.SaveChangesAsync();

            // Act
            var response = await _client.GetAsync("/api/v1/teachers/pagination?pageNumber=1&pageSize=2");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            var paginatedResponse = JsonSerializer.Deserialize<PaginatedResponse<IEnumerable<Teacher>>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            paginatedResponse.Should().NotBeNull();
            paginatedResponse.Success.Should().BeTrue();
            paginatedResponse.Data.Should().NotBeNull();
            paginatedResponse.Data.Should().HaveCount(2);
            paginatedResponse.Pagination.Should().NotBeNull();
            paginatedResponse.Pagination.TotalRecords.Should().Be(3);
            paginatedResponse.Pagination.Page.Should().Be(1);
            paginatedResponse.Pagination.PageSize.Should().Be(2);
            paginatedResponse.Pagination.TotalPages.Should().Be(2);
        }

        [Fact]
        public async Task GetTeachersPagination_ShouldReturnBadRequest_WhenInvalidParametersProvided()
        {
            // Act
            var response = await _client.GetAsync("/api/v1/teachers/pagination?pageNumber=0&pageSize=0");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
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
                Description = "Basic mathematics questions",
                TeacherId = 1,
                IsActive = false
            };

            _context.teachers.Add(teacher);
            _context.quizzes.Add(quiz);
            await _context.SaveChangesAsync();

            // Act
            var response = await _client.PostAsync($"/api/v1/teachers/{teacher.Id}/quizzes/{quiz.Id}/start");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<ApiResponse<Quiz>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            apiResponse.Should().NotBeNull();
            apiResponse.Success.Should().BeTrue();
            apiResponse.Data.Should().NotBeNull();
            apiResponse.Data.IsActive.Should().BeTrue();
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
                Description = "Basic mathematics questions",
                TeacherId = 1,
                IsActive = true
            };

            _context.teachers.Add(teacher);
            _context.quizzes.Add(quiz);
            await _context.SaveChangesAsync();

            // Act
            var response = await _client.PostAsync($"/api/v1/teachers/{teacher.Id}/quizzes/{quiz.Id}/end");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<ApiResponse<Quiz>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            apiResponse.Should().NotBeNull();
            apiResponse.Success.Should().BeTrue();
            apiResponse.Data.Should().NotBeNull();
            apiResponse.Data.IsActive.Should().BeFalse();
        }
    }
} 