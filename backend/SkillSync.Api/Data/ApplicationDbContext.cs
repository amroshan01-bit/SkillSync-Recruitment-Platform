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

    public DbSet<JobSeekerProfile> JobSeekerProfiles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<JobSeekerProfile>()
            .HasIndex(profile => profile.UserId)
            .IsUnique();
    }
}