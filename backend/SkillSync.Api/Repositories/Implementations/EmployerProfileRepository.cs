using Microsoft.EntityFrameworkCore;
using SkillSync.Api.Data;
using SkillSync.Api.Models;
using SkillSync.Api.Repositories.Interfaces;

namespace SkillSync.Api.Repositories.Implementations;

public class EmployerProfileRepository : IEmployerProfileRepository
{
    private readonly ApplicationDbContext _context;

    public EmployerProfileRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<EmployerProfile>> GetAllAsync()
    {
        return await _context.EmployerProfiles
            .OrderBy(profile => profile.CompanyName)
            .ToListAsync();
    }

    public async Task<EmployerProfile?> GetByIdAsync(Guid id)
    {
        return await _context.EmployerProfiles
            .FirstOrDefaultAsync(profile => profile.Id == id);
    }

    public async Task<EmployerProfile?> GetByUserIdAsync(Guid userId)
    {
        return await _context.EmployerProfiles
            .FirstOrDefaultAsync(profile => profile.UserId == userId);
    }

    public async Task<bool> ExistsForUserAsync(Guid userId)
    {
        return await _context.EmployerProfiles
            .AnyAsync(profile => profile.UserId == userId);
    }

    public async Task<EmployerProfile> CreateAsync(
        EmployerProfile profile)
    {
        _context.EmployerProfiles.Add(profile);
        await _context.SaveChangesAsync();

        return profile;
    }

    public async Task UpdateAsync(EmployerProfile profile)
    {
        _context.EmployerProfiles.Update(profile);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(EmployerProfile profile)
    {
        _context.EmployerProfiles.Remove(profile);
        await _context.SaveChangesAsync();
    }
}