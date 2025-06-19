using Microsoft.AspNetCore.Mvc.Testing;
using QuizupAPI.Models;
using QuizupAPI.Models.DTOs.Quiz;
using QuizupAPI.Models.DTOs.Question;
using QuizupAPI.Models.DTOs.Response;
using System.Net;
using System.Text;
using System.Text.Json;

namespace QuizupAPI.Test
{
    public class QuizzesControllerIntegrationTests : IntegrationTestBase
    {
        public QuizzesControllerIntegrationTests(WebApplicationFactory<Program> factory) : base(factory)
        {
        }

        [Fact]
        public async Task GetAllQuizzes_ShouldReturnEmptyList_WhenNoQuizzesExist()
        {
            // Act
            var response = await _client.GetAsync("/api/v1/quizzes");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<ApiResponse<IEnumerable<Quiz>>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            apiResponse.Should().NotBeNull();
            apiResponse.Success.Should().BeTrue();
            apiResponse.Data.Should().NotBeNull();
            apiResponse.Data.Should().BeEmpty();
        }

        [Fact]
        public async Task CreateQuiz_ShouldReturnCreatedQuiz_WhenValidDataProvided()
        {
            // Arrange
            var quizDto = new QuizAddRequestDTO
            {
                Title = "Math Quiz",
                Description = "Basic mathematics questions",
                TeacherId = 1,
                DueDate = DateTime.Now.AddDays(7)
            };

            var json = JsonSerializer.Serialize(quizDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/v1/quizzes", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var responseContent = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<ApiResponse<Quiz>>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            apiResponse.Should().NotBeNull();
            apiResponse.Success.Should().BeTrue();
            apiResponse.Data.Should().NotBeNull();
            apiResponse.Data.Title.Should().Be(quizDto.Title);
            apiResponse.Data.Description.Should().Be(quizDto.Description);
            apiResponse.Data.TeacherId.Should().Be(quizDto.TeacherId);
            apiResponse.Data.Id.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task CreateQuiz_ShouldReturnBadRequest_WhenRequiredFieldsAreMissing()
        {
            // Arrange
            var quizDto = new QuizAddRequestDTO
            {
            };

            var json = JsonSerializer.Serialize(quizDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/v1/quizzes", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GetQuizById_ShouldReturnQuiz_WhenQuizExists()
        {
            // Arrange
            var quiz = new Quiz
            {
                Title = "Math Quiz",
                Description = "Basic mathematics questions",
                TeacherId = 1,
                IsActive = true
            };

            _context.quizzes.Add(quiz);
            await _context.SaveChangesAsync();

            // Act
            var response = await _client.GetAsync($"/api/v1/quizzes/{quiz.Id}");

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
            apiResponse.Data.Id.Should().Be(quiz.Id);
            apiResponse.Data.Title.Should().Be(quiz.Title);
        }

        [Fact]
        public async Task GetQuizById_ShouldReturnNotFound_WhenQuizDoesNotExist()
        {
            // Act
            var response = await _client.GetAsync("/api/v1/quizzes/999");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task UpdateQuiz_ShouldReturnUpdatedQuiz_WhenValidDataProvided()
        {
            // Arrange
            var quiz = new Quiz
            {
                Title = "Math Quiz",
                Description = "Basic mathematics questions",
                TeacherId = 1,
                IsActive = true
            };

            _context.quizzes.Add(quiz);
            await _context.SaveChangesAsync();

            var updateDto = new QuizUpdateRequestDTO
            {
                Title = "Advanced Math Quiz",
                Description = "Advanced mathematics questions",
                DueDate = DateTime.Now.AddDays(14)
            };

            var json = JsonSerializer.Serialize(updateDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync($"/api/v1/quizzes/{quiz.Id}?teacherId={quiz.TeacherId}", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseContent = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<ApiResponse<Quiz>>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            apiResponse.Should().NotBeNull();
            apiResponse.Success.Should().BeTrue();
            apiResponse.Data.Should().NotBeNull();
            apiResponse.Data.Title.Should().Be(updateDto.Title);
            apiResponse.Data.Description.Should().Be(updateDto.Description);
        }

        [Fact]
        public async Task UpdateQuiz_ShouldReturnNotFound_WhenQuizDoesNotExist()
        {
            // Arrange
            var updateDto = new QuizUpdateRequestDTO
            {
                Title = "Updated Quiz",
                Description = "Updated description"
            };

            var json = JsonSerializer.Serialize(updateDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync("/api/v1/quizzes/999?teacherId=1", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteQuiz_ShouldReturnDeletedQuiz_WhenQuizExists()
        {
            // Arrange
            var quiz = new Quiz
            {
                Title = "Math Quiz",
                Description = "Basic mathematics questions",
                TeacherId = 1,
                IsActive = true
            };

            _context.quizzes.Add(quiz);
            await _context.SaveChangesAsync();

            // Act
            var response = await _client.DeleteAsync($"/api/v1/quizzes/{quiz.Id}?teacherId={quiz.TeacherId}");

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
            apiResponse.Data.Id.Should().Be(quiz.Id);

            var getResponse = await _client.GetAsync($"/api/v1/quizzes/{quiz.Id}");
            getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteQuiz_ShouldReturnNotFound_WhenQuizDoesNotExist()
        {
            // Act
            var response = await _client.DeleteAsync("/api/v1/quizzes/999?teacherId=1");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetAllQuizzes_ShouldReturnQuizzes_WhenQuizzesExist()
        {
            // Arrange
            var quizzes = new List<Quiz>
            {
                new Quiz { Title = "Math Quiz", Description = "Mathematics questions", TeacherId = 1, IsActive = true },
                new Quiz { Title = "Science Quiz", Description = "Science questions", TeacherId = 2, IsActive = false }
            };

            _context.quizzes.AddRange(quizzes);
            await _context.SaveChangesAsync();

            // Act
            var response = await _client.GetAsync("/api/v1/quizzes");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<ApiResponse<IEnumerable<Quiz>>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            apiResponse.Should().NotBeNull();
            apiResponse.Success.Should().BeTrue();
            apiResponse.Data.Should().NotBeNull();
            apiResponse.Data.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetQuizzesPagination_ShouldReturnPaginatedQuizzes_WhenValidParametersProvided()
        {
            // Arrange
            var quizzes = new List<Quiz>
            {
                new Quiz { Title = "Math Quiz", Description = "Mathematics questions", TeacherId = 1, IsActive = true },
                new Quiz { Title = "Science Quiz", Description = "Science questions", TeacherId = 2, IsActive = false },
                new Quiz { Title = "History Quiz", Description = "History questions", TeacherId = 3, IsActive = true }
            };

            _context.quizzes.AddRange(quizzes);
            await _context.SaveChangesAsync();

            // Act
            var response = await _client.GetAsync("/api/v1/quizzes/pagination?pageNumber=1&pageSize=2");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            var paginatedResponse = JsonSerializer.Deserialize<PaginatedResponse<IEnumerable<Quiz>>>(content, new JsonSerializerOptions
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
        public async Task GetQuizzesPagination_ShouldReturnBadRequest_WhenInvalidParametersProvided()
        {
            // Act
            var response = await _client.GetAsync("/api/v1/quizzes/pagination?pageNumber=0&pageSize=0");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task AddQuestionToQuiz_ShouldReturnCreatedQuestion_WhenValidDataProvided()
        {
            // Arrange
            var quiz = new Quiz
            {
                Title = "Math Quiz",
                Description = "Basic mathematics questions",
                TeacherId = 1,
                IsActive = true
            };

            _context.quizzes.Add(quiz);
            await _context.SaveChangesAsync();

            var questionDto = new QuestionAddRequestDTO
            {
                Text = "What is 2 + 2?",
                Options = new List<string> { "3", "4", "5", "6" },
                CorrectAnswer = 1,
                Points = 10
            };

            var json = JsonSerializer.Serialize(questionDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync($"/api/v1/quizzes/{quiz.Id}/questions?teacherId={quiz.TeacherId}", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var responseContent = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<ApiResponse<Question>>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            apiResponse.Should().NotBeNull();
            apiResponse.Success.Should().BeTrue();
            apiResponse.Data.Should().NotBeNull();
            apiResponse.Data.Text.Should().Be(questionDto.Text);
            apiResponse.Data.Points.Should().Be(questionDto.Points);
            apiResponse.Data.Id.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task UpdateQuestion_ShouldReturnUpdatedQuestion_WhenValidDataProvided()
        {
            // Arrange
            var quiz = new Quiz
            {
                Title = "Math Quiz",
                Description = "Basic mathematics questions",
                TeacherId = 1,
                IsActive = true
            };

            var question = new Question
            {
                Text = "What is 2 + 2?",
                Options = new List<string> { "3", "4", "5", "6" },
                CorrectAnswer = 1,
                Points = 10
            };

            _context.quizzes.Add(quiz);
            _context.questions.Add(question);
            await _context.SaveChangesAsync();

            var updateDto = new QuestionUpdateRequestDTO
            {
                Text = "What is 3 + 3?",
                Options = new List<string> { "5", "6", "7", "8" },
                CorrectAnswer = 1,
                Points = 15
            };

            var json = JsonSerializer.Serialize(updateDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync($"/api/v1/quizzes/{question.Id}/questions?teacherId={quiz.TeacherId}", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseContent = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<ApiResponse<Question>>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            apiResponse.Should().NotBeNull();
            apiResponse.Success.Should().BeTrue();
            apiResponse.Data.Should().NotBeNull();
            apiResponse.Data.Text.Should().Be(updateDto.Text);
            apiResponse.Data.Points.Should().Be(updateDto.Points);
        }
    }
} 