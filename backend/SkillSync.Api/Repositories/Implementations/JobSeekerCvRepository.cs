using Microsoft.EntityFrameworkCore;
using SkillSync.Api.Data;
using SkillSync.Api.Models;
using SkillSync.Api.Repositories.Interfaces;

namespace SkillSync.Api.Repositories.Implementations;

public class JobSeekerCvRepository : IJobSeekerCvRepository
{
    private readonly ApplicationDbContext _context;

    public JobSeekerCvRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<JobSeekerCv?> GetByUserIdAsync(Guid userId)
    {
        return _context.JobSeekerCvs
            .FirstOrDefaultAsync(cv => cv.UserId == userId);
    }

    public async Task<JobSeekerCv> CreateAsync(JobSeekerCv cv)
    {
        _context.JobSeekerCvs.Add(cv);
        await _context.SaveChangesAsync();
        return cv;
    }

    public async Task DeleteAsync(JobSeekerCv cv)
    {
        _context.JobSeekerCvs.Remove(cv);
        await _context.SaveChangesAsync();
    }
}
