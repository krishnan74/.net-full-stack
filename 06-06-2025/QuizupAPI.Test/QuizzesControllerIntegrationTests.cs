using Microsoft.AspNetCore.Mvc.Testing;
using QuizupAPI.Models;
using QuizupAPI.Models.DTOs.Quiz;
using QuizupAPI.Models.DTOs.Question;
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
            var quizzes = JsonSerializer.Deserialize<List<Quiz>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            quizzes.Should().NotBeNull();
            quizzes.Should().BeEmpty();
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
            var createdQuiz = JsonSerializer.Deserialize<Quiz>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            createdQuiz.Should().NotBeNull();
            createdQuiz.Title.Should().Be(quizDto.Title);
            createdQuiz.Description.Should().Be(quizDto.Description);
            createdQuiz.TeacherId.Should().Be(quizDto.TeacherId);
            createdQuiz.Id.Should().BeGreaterThan(0);
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
            var retrievedQuiz = JsonSerializer.Deserialize<Quiz>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            retrievedQuiz.Should().NotBeNull();
            retrievedQuiz.Id.Should().Be(quiz.Id);
            retrievedQuiz.Title.Should().Be(quiz.Title);
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
            var updatedQuiz = JsonSerializer.Deserialize<Quiz>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            updatedQuiz.Should().NotBeNull();
            updatedQuiz.Title.Should().Be(updateDto.Title);
            updatedQuiz.Description.Should().Be(updateDto.Description);
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
            var deletedQuiz = JsonSerializer.Deserialize<Quiz>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            deletedQuiz.Should().NotBeNull();
            deletedQuiz.Id.Should().Be(quiz.Id);

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
            var retrievedQuizzes = JsonSerializer.Deserialize<List<Quiz>>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            retrievedQuizzes.Should().NotBeNull();
            retrievedQuizzes.Should().HaveCount(2);
            retrievedQuizzes.Should().Contain(q => q.Title == "Math Quiz");
            retrievedQuizzes.Should().Contain(q => q.Title == "Science Quiz");
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
                CorrectAnswer = "4",
                Options = new List<string> { "3", "4", "5", "6" }
            };

            var json = JsonSerializer.Serialize(questionDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync($"/api/v1/quizzes/{quiz.Id}/questions?teacherId={quiz.TeacherId}", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var responseContent = await response.Content.ReadAsStringAsync();
            var createdQuestion = JsonSerializer.Deserialize<Question>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            createdQuestion.Should().NotBeNull();
            createdQuestion.Text.Should().Be(questionDto.Text);
            createdQuestion.CorrectAnswer.Should().Be(questionDto.CorrectAnswer);
            createdQuestion.Id.Should().BeGreaterThan(0);
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
                CorrectAnswer = "4"
            };

            _context.quizzes.Add(quiz);
            _context.questions.Add(question);
            await _context.SaveChangesAsync();

            var updateDto = new QuestionUpdateRequestDTO
            {
                Text = "What is 3 + 3?",
                CorrectAnswer = "6",
                Options = new List<string> { "5", "6", "7", "8" }
            };

            var json = JsonSerializer.Serialize(updateDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync($"/api/v1/quizzes/{quiz.Id}/questions?teacherId={quiz.TeacherId}", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseContent = await response.Content.ReadAsStringAsync();
            var updatedQuestion = JsonSerializer.Deserialize<Question>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            updatedQuestion.Should().NotBeNull();
            updatedQuestion.Text.Should().Be(updateDto.Text);
            updatedQuestion.CorrectAnswer.Should().Be(updateDto.CorrectAnswer);
        }
    }
} 