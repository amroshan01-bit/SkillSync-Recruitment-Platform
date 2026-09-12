using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;
using SkillSync.Api.Models;
using Microsoft.EntityFrameworkCore;
using SkillSync.Api.Data;
using SkillSync.Api.Repositories.Implementations;
using SkillSync.Api.Repositories.Interfaces;
using SkillSync.Api.Services.Implementations;
using SkillSync.Api.Services.Interfaces;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Enter the JWT token."
        });

    options.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
});
var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException(
        "JWT key is not configured.");

var jwtIssuer = builder.Configuration["Jwt:Issuer"]
    ?? throw new InvalidOperationException(
        "JWT issuer is not configured.");

var jwtAudience = builder.Configuration["Jwt:Audience"]
    ?? throw new InvalidOperationException(
        "JWT audience is not configured.");

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme =
            JwtBearerDefaults.AuthenticationScheme;

        options.DefaultChallengeScheme =
            JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtIssuer,

                ValidateAudience = true,
                ValidAudience = jwtAudience,

                ValidateLifetime = true,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtKey)),

                ClockSkew = TimeSpan.Zero
            };
    });

builder.Services.AddAuthorization();


// SQL Server Database connection.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString(
            "DefaultConnection")));

// User account repository registration.
builder.Services.AddScoped<IUserRepository, UserRepository>();

// JWT token service registration.
builder.Services.AddScoped<ITokenService, TokenService>();

// Password hashing service registration.
builder.Services.AddScoped<
    IPasswordHasher<User>,
    PasswordHasher<User>>();

// Authentication service registration.
builder.Services.AddScoped<IAuthService, AuthService>();

// Admin management service registration.
builder.Services.AddScoped<IAdminService, AdminService>();

// Job Seeker Profile Repository registration.
builder.Services.AddScoped<
    IJobSeekerProfileRepository,
    JobSeekerProfileRepository>();

// Job Seeker Profile Service registration.
builder.Services.AddScoped<
    IJobSeekerProfileService,
    JobSeekerProfileService>();

// Job Seeker CV Repository registration.
builder.Services.AddScoped<
    IJobSeekerCvRepository,
    JobSeekerCvRepository>();

// Job Seeker CV Service registration.
builder.Services.AddScoped<
    IJobSeekerCvService,
    JobSeekerCvService>();

// Employer Profile Repository registration.
builder.Services.AddScoped<
    IEmployerProfileRepository,
    EmployerProfileRepository>();

// Employer Profile Service registration.
builder.Services.AddScoped<
    IEmployerProfileService,
    EmployerProfileService>();

// Vacancy Repository registration.
builder.Services.AddScoped<
    IVacancyRepository,
    VacancyRepository>();

// Vacancy Service registration.
builder.Services.AddScoped<
    IVacancyService,
    VacancyService>();

// Matching Repository registration.
builder.Services.AddScoped<
    IMatchingRepository,
    MatchingRepository>();

// Matching Service registration.
builder.Services.AddScoped<
    IMatchingService,
    MatchingService>();

// Job Application Repository registration.
builder.Services.AddScoped<
    IJobApplicationRepository,
    JobApplicationRepository>();

// Job Application Service registration.
builder.Services.AddScoped<
    IJobApplicationService,
    JobApplicationService>();

// Angular frontend permission.
builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Swagger is available during development.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("Frontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();