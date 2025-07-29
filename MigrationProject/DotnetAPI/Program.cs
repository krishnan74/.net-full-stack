using System.Text;
using ChienVHShopOnline.Contexts;
using ChienVHShopOnline.Interfaces;
using ChienVHShopOnline.Misc;
using ChienVHShopOnline.Models;
using ChienVHShopOnline.Repositories;
using ChienVHShopOnline.Services;
// using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
// using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Npgsql.Replication.PgOutput.Messages;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "ChienVHShopOnline API", Version = "v1" });
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
                    opts.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
                    opts.JsonSerializerOptions.WriteIndented = true;
                });



builder.Services.AddDbContext<ChienVHShopOnlineContext>(opts =>
{
    opts.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

#region  Repositories
builder.Services.AddTransient<IRepository<int, Category>, CategoryRepository>();

#endregion

#region Services
builder.Services.AddTransient<ICategoryService, CategoryService>();

#endregion

#region AuthenticationFilter
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
#endregion

#region  Misc
// builder.Services.AddAutoMapper(typeof(User));
#endregion


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseAuthentication();
// app.UseAuthorization();
app.MapControllers();

app.Run();

