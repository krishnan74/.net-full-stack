using BankApplication.Contexts;
using BankApplication.Interfaces;
using BankApplication.Repositories;
using BankApplication.Services;

using BankApplication.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers()
                .AddJsonOptions(opts =>
                {
                    opts.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
                    opts.JsonSerializerOptions.WriteIndented = true;
                });



builder.Services.AddDbContext<BankContext>(opts =>
{
    opts.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddTransient<IRepository<int, Bank>, BankRepository>();
builder.Services.AddTransient<IRepository<int, Branch>, BranchRepository>();
builder.Services.AddTransient<IRepository<string, Account>, AccountRepository>();
builder.Services.AddTransient<IRepository<int, Transaction>, TransactionRepository>();
builder.Services.AddTransient<IRepository<int, User>, UserRepository>();


builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IAccountService, AccountService>();
builder.Services.AddTransient<IAccountTransactionService, AccountTransactionService>();
builder.Services.AddTransient<IBankService, BankService>();
builder.Services.AddTransient<IBranchService, BranchService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.MapControllers();

app.Run();


