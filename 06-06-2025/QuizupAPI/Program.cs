using System.Text;
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
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "Quizup API", Version = "v1" });
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

// Adding services

builder.Services.AddControllers()
                .AddJsonOptions(opts =>
                {
                    opts.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
                    opts.JsonSerializerOptions.WriteIndented = true;
                });




builder.Services.AddDbContext<QuizContext>((serviceProvider, opts) =>
{
    opts.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
    opts.AddInterceptors(new DatabasePerformanceInterceptor());
});

builder.Services.AddRateLimiter(
    rateLimiterOptions =>
    {
        rateLimiterOptions.AddPolicy<string>("UserBasedPolicy", context =>
        {
            var user = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if (!string.IsNullOrEmpty(user))
            {
                return RateLimitPartition.GetFixedWindowLimiter(user, _ => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = 1000,
                    Window = TimeSpan.FromHours(1),
                    QueueLimit = 0
                });
            }
            
            // For unauthenticated requests
            var ipAddress = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            return RateLimitPartition.GetFixedWindowLimiter(ipAddress, _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 100, 
                Window = TimeSpan.FromHours(1),
                QueueLimit = 0
            });
        });
        
        rateLimiterOptions.OnRejected = async (context, token) =>
        {
            var user = context.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "anonymous";
            var ipAddress = context.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            
            context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
            context.HttpContext.Response.ContentType = "application/json";
            
            var response = new
            {
                message = "Rate limit exceeded. Please try again later.",
                retryAfter = context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter) 
                    ? retryAfter.TotalSeconds 
                    : 3600,
                user = user,
                ipAddress = ipAddress
            };
            
            await context.HttpContext.Response.WriteAsJsonAsync(response, token);
            
            // Logging the rate limit exceeded
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
            logger.LogWarning(
                $"Rate limit exceeded for user: {user}, IP: {ipAddress}, Path: {context.HttpContext.Request.Path}, Method: {context.HttpContext.Request.Method}"
            );
        };
    }
);


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


// Add JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = false,
        ValidateIssuer = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Keys:JwtTokenKey"]))
    };
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://127.0.0.1:5500")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddSignalR();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseAuthentication();
app.UseAuthorization();
app.UseCors();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();

app.UseSerilogRequestLogging();

app.MapHub<QuizNotificationHub>("/quizNotificationHub");

app.UseHttpsRedirection();
app.UseRateLimiter();

app.MapControllers().RequireRateLimiting("UserBasedPolicy");

app.Run();
