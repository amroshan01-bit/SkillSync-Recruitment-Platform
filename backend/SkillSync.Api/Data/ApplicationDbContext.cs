using Microsoft.EntityFrameworkCore;

namespace SkillSync.Api.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
}