using Microsoft.EntityFrameworkCore;
using QuizupAPI.Contexts;
using QuizupAPI.Hubs;
using QuizupAPI.Interfaces;
using QuizupAPI.Models;
using QuizupAPI.Services;
using QuizupAPI.Repositories;
using QuizupAPI.Middleware;
using QuizupAPI.Interceptors;
using Serilog;
using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

// Adding services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<QuizContext>((serviceProvider, opts) =>
{
    opts.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
    opts.AddInterceptors(new DatabasePerformanceInterceptor());
});

builder.Services.AddRateLimiter(
    rateLimiterOptions =>
    {
        rateLimiterOptions.AddFixedWindowLimiter("fixed", options =>
        {
            options.PermitLimit = 1000;
            options.Window = TimeSpan.FromMinutes(60);
            options.QueueLimit = 0;
        });
    }
);

builder.Services.AddSignalR();

// Register repositories
builder.Services.AddTransient<IRepository<long, Student>, StudentRepository>();
builder.Services.AddTransient<IRepository<long, Teacher>, TeacherRepository>();
builder.Services.AddTransient<IRepository<string, User>, UserRepository>();
builder.Services.AddTransient<IRepository<long, Quiz>, QuizRepository>();
builder.Services.AddTransient<IRepository<long, Question>, QuestionRepository>();
builder.Services.AddTransient<IRepository<long, QuizQuestion>, QuizQuestionRepository>();
builder.Services.AddTransient<IRepository<long, QuizSubmission>, QuizSubmissionRepository>();
builder.Services.AddTransient<IRepository<long, Answer>, AnswerRepository>();

// Register services
builder.Services.AddTransient<IStudentService, StudentService>();
builder.Services.AddTransient<ITeacherService, TeacherService>();
builder.Services.AddTransient<IQuizService, QuizService>();
builder.Services.AddTransient<IAuthenticationService, AuthenticationService>();
builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddTransient<IEncryptionService, EncryptionService>();

builder.Services.AddSingleton<IPerformanceMonitoringService, PerformanceMonitoringService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();

app.UseSerilogRequestLogging();

app.MapHub<QuizNotificationHub>("/quizNotificationHub");

app.UseHttpsRedirection();
app.UseRateLimiter();

app.MapControllers();


app.Run();

public partial class Program { }