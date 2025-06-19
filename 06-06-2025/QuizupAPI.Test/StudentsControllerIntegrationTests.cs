using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using QuizupAPI.Contexts;
using QuizupAPI.Models;
using QuizupAPI.Models.DTOs.Student;
using QuizupAPI.Models.DTOs.Response;
using System.Net;
using System.Text;
using System.Text.Json;

namespace QuizupAPI.Test
{
    public class StudentsControllerIntegrationTests : IntegrationTestBase
    {
        public StudentsControllerIntegrationTests(WebApplicationFactory<Program> factory) : base(factory)
        {
        }

        [Fact]
        public async Task GetAllStudents_ShouldReturnEmptyList_WhenNoStudentsExist()
        {
            // Act
            var response = await _client.GetAsync("/api/v1/students");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<ApiResponse<IEnumerable<Student>>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            apiResponse.Should().NotBeNull();
            apiResponse.Success.Should().BeTrue();
            apiResponse.Data.Should().NotBeNull();
            apiResponse.Data.Should().BeEmpty();
        }

        [Fact]
        public async Task CreateStudent_ShouldReturnCreatedStudent_WhenValidDataProvided()
        {
            // Arrange
            var studentDto = new StudentAddRequestDTO
            {
                Email = "akashraj@gmail.com",
                Password = "password123",
                FirstName = "Akash",
                LastName = "Raj",
                Class = "10A"
            };

            var json = JsonSerializer.Serialize(studentDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/v1/students", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var responseContent = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<ApiResponse<Student>>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            apiResponse.Should().NotBeNull();
            apiResponse.Success.Should().BeTrue();
            apiResponse.Data.Should().NotBeNull();
            apiResponse.Data.Email.Should().Be(studentDto.Email);
            apiResponse.Data.FirstName.Should().Be(studentDto.FirstName);
            apiResponse.Data.LastName.Should().Be(studentDto.LastName);
            apiResponse.Data.Class.Should().Be(studentDto.Class);
            apiResponse.Data.Id.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task CreateStudent_ShouldReturnBadRequest_WhenInvalidEmailProvided()
        {
            // Arrange
            var studentDto = new StudentAddRequestDTO
            {
                Email = "invalid-email",
                Password = "password123",
                FirstName = "Akash",
                LastName = "Raj",
                Class = "10A"
            };

            var json = JsonSerializer.Serialize(studentDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/v1/students", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task CreateStudent_ShouldReturnBadRequest_WhenRequiredFieldsAreMissing()
        {
            // Arrange
            var studentDto = new StudentAddRequestDTO
            {
                Email = "akashraj@gmail.com",
            };

            var json = JsonSerializer.Serialize(studentDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/v1/students", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GetStudentById_ShouldReturnStudent_WhenStudentExists()
        {
            // Arrange
            var student = new Student
            {
                Email = "akashraj@gmail.com",
                FirstName = "Akash",
                LastName = "Raj",
                Class = "10A"
            };

            _context.students.Add(student);
            await _context.SaveChangesAsync();

            // Act
            var response = await _client.GetAsync($"/api/v1/students/{student.Id}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<ApiResponse<Student>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            apiResponse.Should().NotBeNull();
            apiResponse.Success.Should().BeTrue();
            apiResponse.Data.Should().NotBeNull();
            apiResponse.Data.Id.Should().Be(student.Id);
            apiResponse.Data.Email.Should().Be(student.Email);
        }

        [Fact]
        public async Task GetStudentById_ShouldReturnNotFound_WhenStudentDoesNotExist()
        {
            // Act
            var response = await _client.GetAsync("/api/v1/students/999");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task UpdateStudent_ShouldReturnUpdatedStudent_WhenValidDataProvided()
        {
            // Arrange
            var student = new Student
            {
                Email = "akashraj@gmail.com",
                FirstName = "Akash",
                LastName = "Raj",
                Class = "10A"
            };

            _context.students.Add(student);
            await _context.SaveChangesAsync();

            var updateDto = new StudentUpdateRequestDTO
            {
                FirstName = "Jane",
                LastName = "Smith",
                Class = "11B"
            };

            var json = JsonSerializer.Serialize(updateDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync($"/api/v1/students/{student.Id}", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseContent = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<ApiResponse<Student>>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            apiResponse.Should().NotBeNull();
            apiResponse.Success.Should().BeTrue();
            apiResponse.Data.Should().NotBeNull();
            apiResponse.Data.FirstName.Should().Be(updateDto.FirstName);
            apiResponse.Data.LastName.Should().Be(updateDto.LastName);
            apiResponse.Data.Class.Should().Be(updateDto.Class);
        }

        [Fact]
        public async Task UpdateStudent_ShouldReturnNotFound_WhenStudentDoesNotExist()
        {
            // Arrange
            var updateDto = new StudentUpdateRequestDTO
            {
                FirstName = "Jane",
                LastName = "Smith",
                Class = "11B"
            };

            var json = JsonSerializer.Serialize(updateDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync("/api/v1/students/999", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteStudent_ShouldReturnDeletedStudent_WhenStudentExists()
        {
            // Arrange
            var student = new Student
            {
                Email = "akashraj@gmail.com",
                FirstName = "Akash",
                LastName = "Raj",
                Class = "12C"
            };

            _context.students.Add(student);
            await _context.SaveChangesAsync();

            // Act
            var response = await _client.DeleteAsync($"/api/v1/students/{student.Id}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<ApiResponse<Student>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            apiResponse.Should().NotBeNull();
            apiResponse.Success.Should().BeTrue();
            apiResponse.Data.Should().NotBeNull();
            apiResponse.Data.Id.Should().Be(student.Id);

            var getResponse = await _client.GetAsync($"/api/v1/students/{student.Id}");
            getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteStudent_ShouldReturnNotFound_WhenStudentDoesNotExist()
        {
            // Act
            var response = await _client.DeleteAsync("/api/v1/students/999");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetAllStudents_ShouldReturnStudents_WhenStudentsExist()
        {
            // Arrange
            var students = new List<Student>
            {
                new Student { Email = "student1@gmail.com", FirstName = "John", LastName = "Doe", Class = "10A" },
                new Student { Email = "student2@gmail.com", FirstName = "Jane", LastName = "Smith", Class = "11B" }
            };

            _context.students.AddRange(students);
            await _context.SaveChangesAsync();

            // Act
            var response = await _client.GetAsync("/api/v1/students");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<ApiResponse<IEnumerable<Student>>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            apiResponse.Should().NotBeNull();
            apiResponse.Success.Should().BeTrue();
            apiResponse.Data.Should().NotBeNull();
            apiResponse.Data.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetStudentsPagination_ShouldReturnPaginatedStudents_WhenValidParametersProvided()
        {
            // Arrange
            var students = new List<Student>
            {
                new Student { Email = "student1@gmail.com", FirstName = "John", LastName = "Doe", Class = "10A" },
                new Student { Email = "student2@gmail.com", FirstName = "Jane", LastName = "Smith", Class = "11B" },
                new Student { Email = "student3@gmail.com", FirstName = "Bob", LastName = "Johnson", Class = "12C" }
            };

            _context.students.AddRange(students);
            await _context.SaveChangesAsync();

            // Act
            var response = await _client.GetAsync("/api/v1/students/pagination?pageNumber=1&pageSize=2");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            var paginatedResponse = JsonSerializer.Deserialize<PaginatedResponse<IEnumerable<Student>>>(content, new JsonSerializerOptions
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
        public async Task GetStudentsPagination_ShouldReturnBadRequest_WhenInvalidParametersProvided()
        {
            // Act
            var response = await _client.GetAsync("/api/v1/students/pagination?pageNumber=0&pageSize=0");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
} 