using Microsoft.EntityFrameworkCore;
using QuizupAPI.Contexts;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<QuizContext>(opts =>
{
    opts.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}

app.UseHttpsRedirection();

app.Run();