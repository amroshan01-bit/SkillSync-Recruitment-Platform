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

    // Database rules.
    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ஒரு user-க்கு ஒரு profile மட்டும்.
        modelBuilder.Entity<JobSeekerProfile>()
            .HasIndex(profile => profile.UserId)
            .IsUnique();

        // ஒரு user-க்கு ஒரு current CV மட்டும்.
        modelBuilder.Entity<JobSeekerCv>()
            .HasIndex(cv => cv.UserId)
            .IsUnique();
    }
}