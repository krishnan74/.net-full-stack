using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using QuizupAPI.Contexts;
using QuizupAPI.Models;
using QuizupAPI.Models.DTOs.Student;
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
            var students = JsonSerializer.Deserialize<List<Student>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            students.Should().NotBeNull();
            students.Should().BeEmpty();
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
            var createdStudent = JsonSerializer.Deserialize<Student>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            createdStudent.Should().NotBeNull();
            createdStudent.Email.Should().Be(studentDto.Email);
            createdStudent.FirstName.Should().Be(studentDto.FirstName);
            createdStudent.LastName.Should().Be(studentDto.LastName);
            createdStudent.Class.Should().Be(studentDto.Class);
            createdStudent.Id.Should().BeGreaterThan(0);
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
            var retrievedStudent = JsonSerializer.Deserialize<Student>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            retrievedStudent.Should().NotBeNull();
            retrievedStudent.Id.Should().Be(student.Id);
            retrievedStudent.Email.Should().Be(student.Email);
        }

        [Fact]
        public async Task GetStudentById_ShouldReturnNotFound_WhenStudentRajsNotExist()
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
            var updatedStudent = JsonSerializer.Deserialize<Student>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            updatedStudent.Should().NotBeNull();
            updatedStudent.FirstName.Should().Be(updateDto.FirstName);
            updatedStudent.LastName.Should().Be(updateDto.LastName);
            updatedStudent.Class.Should().Be(updateDto.Class);
        }

        [Fact]
        public async Task UpdateStudent_ShouldReturnNotFound_WhenStudentRajsNotExist()
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
            var deletedStudent = JsonSerializer.Deserialize<Student>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            deletedStudent.Should().NotBeNull();
            deletedStudent.Id.Should().Be(student.Id);

            var getResponse = await _client.GetAsync($"/api/v1/students/{student.Id}");
            getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteStudent_ShouldReturnNotFound_WhenStudentRajsNotExist()
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
                new Student { Email = "akashraj@gmail.com", FirstName = "Akash", LastName = "Raj", Class = "12C" },
                new Student { Email = "krish@gmail.com", FirstName = "Krish", LastName = "Kumar", Class = "10B" }
            };

            _context.students.AddRange(students);
            await _context.SaveChangesAsync();

            // Act
            var response = await _client.GetAsync("/api/v1/students");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            var retrievedStudents = JsonSerializer.Deserialize<List<Student>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            retrievedStudents.Should().NotBeNull();
            retrievedStudents.Should().HaveCount(2);
            retrievedStudents.Should().Contain(s => s.Email == "akashraj@gmail.com");
            retrievedStudents.Should().Contain(s => s.Email == "krish@gmail.com");
        }
    }
} 