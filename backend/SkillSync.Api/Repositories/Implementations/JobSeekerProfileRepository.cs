using Microsoft.EntityFrameworkCore;
using SkillSync.Api.Data;
using SkillSync.Api.Models;
using SkillSync.Api.Repositories.Interfaces;

namespace SkillSync.Api.Repositories.Implementations;

public class JobSeekerProfileRepository : IJobSeekerProfileRepository
{
    private readonly ApplicationDbContext _context;

    public JobSeekerProfileRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<JobSeekerProfile>> GetAllAsync()
    {
        return await _context.JobSeekerProfiles
            .OrderBy(profile => profile.FullName)
            .ToListAsync();
    }

    public async Task<JobSeekerProfile?> GetByIdAsync(Guid id)
    {
        return await _context.JobSeekerProfiles
            .FirstOrDefaultAsync(profile => profile.Id == id);
    }

    public async Task<JobSeekerProfile?> GetByUserIdAsync(Guid userId)
    {
        return await _context.JobSeekerProfiles
            .FirstOrDefaultAsync(profile => profile.UserId == userId);
    }

    public async Task<bool> ExistsForUserAsync(Guid userId)
    {
        return await _context.JobSeekerProfiles
            .AnyAsync(profile => profile.UserId == userId);
    }

    public async Task<JobSeekerProfile> CreateAsync(
        JobSeekerProfile profile)
    {
        _context.JobSeekerProfiles.Add(profile);
        await _context.SaveChangesAsync();

        return profile;
    }

    public async Task UpdateAsync(JobSeekerProfile profile)
    {
        _context.JobSeekerProfiles.Update(profile);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(JobSeekerProfile profile)
    {
        _context.JobSeekerProfiles.Remove(profile);
        await _context.SaveChangesAsync();
    }
}