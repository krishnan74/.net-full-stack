using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using QuizupAPI.Contexts;
using QuizupAPI.Models;
using QuizupAPI.Interfaces;
using QuizupAPI.Services;
using QuizupAPI.Repositories;

namespace QuizupAPI.Test
{
    public abstract class IntegrationTestBase : IClassFixture<WebApplicationFactory<Program>>, IDisposable
    {
        protected readonly WebApplicationFactory<Program> _factory;
        protected readonly QuizContext _context;
        protected readonly HttpClient _client;

        protected IntegrationTestBase(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Remove the existing DbContext registration
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<QuizContext>));
                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    // Add in-memory database
                    services.AddDbContext<QuizContext>(options =>
                    {
                        options.UseInMemoryDatabase($"TestDb_{Guid.NewGuid()}");
                    });

                    // Ensure repositories and services are registered
                    services.AddScoped<IRepository<long, Student>, StudentRepository>();
                    services.AddScoped<IRepository<long, Teacher>, TeacherRepository>();
                    services.AddScoped<IRepository<string, User>, UserRepository>();
                    services.AddScoped<IRepository<long, Quiz>, QuizRepository>();
                    services.AddScoped<IRepository<long, Question>, QuestionRepository>();
                    services.AddScoped<IRepository<long, QuizQuestion>, QuizQuestionRepository>();
                    services.AddScoped<IRepository<long, QuizSubmission>, QuizSubmissionRepository>();
                    services.AddScoped<IRepository<long, Answer>, AnswerRepository>();

                    services.AddScoped<IStudentService, StudentService>();
                    services.AddScoped<ITeacherService, TeacherService>();
                    services.AddScoped<IQuizService, QuizService>();
                    services.AddScoped<IAuthenticationService, AuthenticationService>();
                    services.AddScoped<ITokenService, TokenService>();
                    services.AddScoped<IEncryptionService, EncryptionService>();
                });
            });

            _client = _factory.CreateClient();
            
            // Get the DbContext from the service provider
            var scope = _factory.Services.CreateScope();
            _context = scope.ServiceProvider.GetRequiredService<QuizContext>();
            
            // Ensure database is created
            _context.Database.EnsureCreated();
        }

        protected async Task SeedTestDataAsync()
        {
            await _context.SaveChangesAsync();
        }

        protected async Task ClearTestDataAsync()
        {
            _context.ChangeTracker.Clear();
            _context.Database.EnsureDeleted();
            await _context.Database.EnsureCreatedAsync();
        }

        public void Dispose()
        {
            _context?.Dispose();
            _client?.Dispose();
        }
    }
} 