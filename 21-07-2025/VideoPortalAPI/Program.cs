using System.Text;
using VideoPortalAPI.Contexts;
using VideoPortalAPI.Interfaces;
using VideoPortalAPI.Misc;
using VideoPortalAPI.Models;
using VideoPortalAPI.Repositories;
using VideoPortalAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Npgsql.Replication.PgOutput.Messages;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "Clinic API", Version = "v1" });
    // opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    // {
    //     In = ParameterLocation.Header,
    //     Description = "Please enter token",
    //     Name = "Authorization",
    //     Type = SecuritySchemeType.Http,
    //     BearerFormat = "JWT",
    //     Scheme = "bearer"
    // });
    // opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    // {
    //     {
    //         new OpenApiSecurityScheme
    //         {
    //             Reference = new OpenApiReference
    //             {
    //                 Type=ReferenceType.SecurityScheme,
    //                 Id="Bearer"
    //             }
    //         },
    //         new string[]{}
    //     }
    // });
});

builder.Services.AddControllers()
                .AddJsonOptions(opts =>
                {
                    opts.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
                    opts.JsonSerializerOptions.WriteIndented = true;
                });



builder.Services.AddDbContext<VideoPortalContext>(opts =>
{
    opts.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});
#region  Repositories
builder.Services.AddTransient<IRepository<int, TrainingVideo>, TrainingVideoRepository>();
builder.Services.AddTransient<IRepository<string, User>, UserRepository>();
#endregion

#region Services
builder.Services.AddTransient<ITrainingVideoService, TrainingVideoService>();
builder.Services.AddTransient<IEncryptionService, EncryptionService>();
builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddTransient<IAuthenticationService, AuthenticationService>();
#endregion

// #region AuthenticationFilter
// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//                 .AddJwtBearer(options =>
//                 {
//                     options.TokenValidationParameters = new TokenValidationParameters
//                     {
//                         ValidateAudience = false,
//                         ValidateIssuer = false,
//                         ValidateLifetime = true,
//                         ValidateIssuerSigningKey = true,
//                         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Keys:JwtTokenKey"]))
//                     };
//                 });
// #endregion


builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(["http://127.0.0.1:5500", "http://127.0.1:4200", "http://localhost:4200", "http://localhost:5500", "http://localhost:8080", "http://127.0.1:8080"])
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});


var app = builder.Build();

app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseAuthentication();
// app.UseAuthorization();
app.MapControllers();

app.Run();

