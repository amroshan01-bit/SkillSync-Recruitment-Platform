using Microsoft.EntityFrameworkCore;
using SkillSync.Api.Models;

namespace SkillSync.Api.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    // Registered user accounts table.
    public DbSet<User> Users
    {
      get;
      set;
    }

    // Job seeker profile table.
    public DbSet<JobSeekerProfile> JobSeekerProfiles
    {
        get;
        set;
    }

    // Job seeker CV details table.
    public DbSet<JobSeekerCv> JobSeekerCvs
    {
        get;
        set;
    }

    // Employer profile table.
    public DbSet<EmployerProfile> EmployerProfiles
    {
        get;
        set;
    }

    // Vacancy table.
    public DbSet<Vacancy> Vacancies
    {
        get;
        set;
    }

    // Job seeker skills table.
    public DbSet<JobSeekerSkill> JobSeekerSkills
    {
        get;
        set;
    }

    // Job applications table.
    public DbSet<JobApplication> JobApplications
    {
        get;
        set;
    }

    // Database rules.
    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Each email address can belong to only one user.
        modelBuilder.Entity<User>()
            .HasIndex(user => user.Email)
            .IsUnique();

        // Each user can have only one job seeker profile.
        modelBuilder.Entity<JobSeekerProfile>()
            .HasIndex(profile => profile.UserId)
            .IsUnique();

        // Each user can have only one current CV.
        modelBuilder.Entity<JobSeekerCv>()
            .HasIndex(cv => cv.UserId)
            .IsUnique();

        // Each user can have only one employer profile.
        modelBuilder.Entity<EmployerProfile>()
            .HasIndex(profile => profile.UserId)
            .IsUnique();

        // One employer profile can create many vacancies.
        modelBuilder.Entity<Vacancy>()
            .HasOne(vacancy => vacancy.EmployerProfile)
            .WithMany()
            .HasForeignKey(vacancy => vacancy.EmployerProfileId)
            .OnDelete(DeleteBehavior.Restrict);

        // Improve employer vacancy search performance.
        modelBuilder.Entity<Vacancy>()
            .HasIndex(vacancy => vacancy.EmployerProfileId);

        // Improve vacancy status filtering performance.
        modelBuilder.Entity<Vacancy>()
            .HasIndex(vacancy => vacancy.Status);

        // Prevent duplicate skills for the same job seeker.
        modelBuilder.Entity<JobSeekerSkill>()
            .HasIndex(skill => new
            {
                skill.UserId,
                skill.SkillName
            })
            .IsUnique();

        // Prevent duplicate applications for the same vacancy.
        modelBuilder.Entity<JobApplication>()
            .HasIndex(application => new
            {
                application.JobSeekerUserId,
                application.VacancyId
            })
            .IsUnique();

        // Each application belongs to one vacancy.
        modelBuilder.Entity<JobApplication>()
            .HasOne(application => application.Vacancy)
            .WithMany()
            .HasForeignKey(application => application.VacancyId)
            .OnDelete(DeleteBehavior.Restrict);

        // Improve application status filtering performance.
        modelBuilder.Entity<JobApplication>()
            .HasIndex(application => application.Status);
    }
}