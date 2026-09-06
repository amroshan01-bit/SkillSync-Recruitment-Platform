using Microsoft.EntityFrameworkCore;
using SkillSync.Api.Data;
using SkillSync.Api.Repositories.Implementations;
using SkillSync.Api.Repositories.Interfaces;
using SkillSync.Api.Services.Implementations;
using SkillSync.Api.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// SQL Server Database connection.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString(
            "DefaultConnection")));

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

app.UseAuthorization();

app.MapControllers();

app.Run();