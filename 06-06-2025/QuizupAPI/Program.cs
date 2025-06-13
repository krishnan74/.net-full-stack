using Microsoft.EntityFrameworkCore;
using QuizupAPI.Contexts;
using QuizupAPI.Hubs;
using QuizupAPI.Interfaces;
using QuizupAPI.Models;
using QuizupAPI.Services;
using QuizupAPI.Repositories;
using Serilog;
using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<QuizContext>(opts =>
{
    opts.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.MapHub<QuizNotificationHub>("/quizNotificationHub");

app.UseHttpsRedirection();
app.UseRateLimiter();

app.MapControllers();

app.Run();

// public partial class Program { }