using Microsoft.EntityFrameworkCore;
using QuizupAPI.Contexts;
using QuizupAPI.Hubs;
using QuizupAPI.Interfaces;
using QuizupAPI.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<QuizContext>(opts =>
{
    opts.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddSignalR();

builder.Services.AddScoped<TeacherService>();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}
app.MapHub<QuizNotificationHub>("/quizNotificationHub");

app.UseHttpsRedirection();

app.Run();