using Microsoft.EntityFrameworkCore;
using QuizupAPI.Contexts;
using QuizupAPI.Hubs;
using QuizupAPI.Interfaces;
using QuizupAPI.Services;
using Serilog;
using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

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

builder.Services.AddScoped<TeacherService>();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}
app.UseSerilogRequestLogging();

app.MapHub<QuizNotificationHub>("/quizNotificationHub");

app.UseHttpsRedirection();
app.UseRateLimiter();

app.Run();