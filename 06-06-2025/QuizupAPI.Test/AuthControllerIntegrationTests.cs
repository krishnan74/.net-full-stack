using Microsoft.AspNetCore.Mvc.Testing;
using QuizupAPI.Models;
using QuizupAPI.Models.DTOs.Authentication;
using QuizupAPI.Models.DTOs.Response;
using System.Net;
using System.Text;
using System.Text.Json;

namespace QuizupAPI.Test
{
    public class AuthControllerIntegrationTests : IntegrationTestBase
    {
        public AuthControllerIntegrationTests(WebApplicationFactory<Program> factory) : base(factory)
        {
        }

        [Fact]
        public async Task Login_ShouldReturnToken_WhenValidCredentialsProvided()
        {
            // Arrange
            var user = new User
            {
                Username = "akashraj@gmail.com",
                HashedPassword = BCrypt.Net.BCrypt.HashPassword("password123"),
                Role = "Student"
            };

            _context.users.Add(user);
            await _context.SaveChangesAsync();

            var loginDto = new UserLoginRequestDTO
            {
                Username = "akashraj@gmail.com",
                Password = "password123"
            };

            var json = JsonSerializer.Serialize(loginDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/v1/auth/login", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseContent = await response.Content.ReadAsStringAsync();
            var apiResponse = JsonSerializer.Deserialize<ApiResponse<UserLoginResponseDTO>>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            apiResponse.Should().NotBeNull();
            apiResponse.Success.Should().BeTrue();
            apiResponse.Data.Should().NotBeNull();
            apiResponse.Data.AccessToken.Should().NotBeNullOrEmpty();
            apiResponse.Data.RefreshToken.Should().NotBeNullOrEmpty();
            apiResponse.Data.Username.Should().Be(loginDto.Username);
        }

        [Fact]
        public async Task Login_ShouldReturnUnauthorized_WhenInvalidCredentialsProvided()
        {
            // Arrange
            var user = new User
            {
                Username = "akashraj@gmail.com",
                HashedPassword = BCrypt.Net.BCrypt.HashPassword("password123"),
                Role = "Student"
            };

            _context.users.Add(user);
            await _context.SaveChangesAsync();

            var loginDto = new UserLoginRequestDTO
            {
                Username = "akashraj@gmail.com",
                Password = "wrongpassword"
            };

            var json = JsonSerializer.Serialize(loginDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/v1/auth/login", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Login_ShouldReturnBadRequest_WhenInvalidEmailProvided()
        {
            // Arrange
            var loginDto = new UserLoginRequestDTO
            {
                Username = "invalid-email",
                Password = "password123"
            };

            var json = JsonSerializer.Serialize(loginDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/v1/auth/login", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Login_ShouldReturnBadRequest_WhenRequiredFieldsAreMissing()
        {
            // Arrange
            var loginDto = new UserLoginRequestDTO
            {
                // Missing email and password
            };

            var json = JsonSerializer.Serialize(loginDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/v1/auth/login", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task RefreshToken_ShouldReturnNewToken_WhenValidRefreshTokenProvided()
        {
            // Arrange
            var user = new User
            {
                Username = "akashraj@gmail.com",
                HashedPassword = BCrypt.Net.BCrypt.HashPassword("password123"),
                Role = "Student"
            };

            _context.users.Add(user);
            await _context.SaveChangesAsync();

            // First, login to get a refresh token
            var loginDto = new UserLoginRequestDTO
            {
                Username = "akashraj@gmail.com",
                Password = "password123"
            };

            var loginJson = JsonSerializer.Serialize(loginDto);
            var loginContent = new StringContent(loginJson, Encoding.UTF8, "application/json");
            var loginResponse = await _client.PostAsync("/api/v1/auth/login", loginContent);
            var loginResponseContent = await loginResponse.Content.ReadAsStringAsync();
            var loginApiResponse = JsonSerializer.Deserialize<ApiResponse<UserLoginResponseDTO>>(loginResponseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            var refreshDto = new RefreshTokenRequestDTO
            {
                RefreshToken = loginApiResponse.Data.RefreshToken
            };

            var refreshJson = JsonSerializer.Serialize(refreshDto);
            var refreshContent = new StringContent(refreshJson, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/v1/auth/refresh", refreshContent);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var responseContent = await response.Content.ReadAsStringAsync();
            var refreshApiResponse = JsonSerializer.Deserialize<ApiResponse<UserLoginResponseDTO>>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            refreshApiResponse.Should().NotBeNull();
            refreshApiResponse.Success.Should().BeTrue();
            refreshApiResponse.Data.Should().NotBeNull();
            refreshApiResponse.Data.AccessToken.Should().NotBeNullOrEmpty();
            refreshApiResponse.Data.RefreshToken.Should().NotBeNullOrEmpty();
            refreshApiResponse.Data.AccessToken.Should().NotBe(loginApiResponse.Data.AccessToken); 
        }

        [Fact]
        public async Task RefreshToken_ShouldReturnUnauthorized_WhenInvalidRefreshTokenProvided()
        {
            // Arrange
            var refreshDto = new RefreshTokenRequestDTO
            {
                RefreshToken = "invalid-refresh-token"
            };

            var json = JsonSerializer.Serialize(refreshDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/v1/auth/refresh", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task RefreshToken_ShouldReturnBadRequest_WhenRefreshTokenIsMissing()
        {
            // Arrange
            var refreshDto = new RefreshTokenRequestDTO
            {
                // Missing refresh token
            };

            var json = JsonSerializer.Serialize(refreshDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/v1/auth/refresh", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GetCurrentUser_ShouldReturnUnauthorized_WhenNoAuthentication()
        {
            // Act
            var response = await _client.GetAsync("/api/v1/auth/me");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task GetCurrentUser_ShouldReturnUserInfo_WhenAuthenticated()
        {
            // Act
            var response = await _client.GetAsync("/api/v1/auth/me");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
} 