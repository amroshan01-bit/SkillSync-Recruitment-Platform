using Microsoft.EntityFrameworkCore;
using SkillSync.Api.Data;
using SkillSync.Api.Models;
using SkillSync.Api.Repositories.Interfaces;

namespace SkillSync.Api.Repositories.Implementations;

public class MatchingRepository : IMatchingRepository
{
    private readonly ApplicationDbContext _context;

    public MatchingRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<JobSeekerSkill>>
        GetSkillsByUserIdAsync(Guid userId)
    {
        return await _context.JobSeekerSkills
            .Where(skill => skill.UserId == userId)
            .OrderBy(skill => skill.SkillName)
            .ToListAsync();
    }

    public async Task ReplaceSkillsAsync(
        Guid userId,
        List<JobSeekerSkill> skills)
    {
        var existingSkills =
            await _context.JobSeekerSkills
                .Where(skill => skill.UserId == userId)
                .ToListAsync();

        _context.JobSeekerSkills.RemoveRange(existingSkills);
        _context.JobSeekerSkills.AddRange(skills);

        await _context.SaveChangesAsync();
    }

    public async Task<List<Vacancy>> GetOpenVacanciesAsync()
    {
        return await _context.Vacancies
            .Include(vacancy => vacancy.EmployerProfile)
            .Where(vacancy =>
                vacancy.Status == "Open" &&
                vacancy.ApplicationClosingDate > DateTime.UtcNow)
            .OrderByDescending(vacancy => vacancy.PostedAtUtc)
            .ToListAsync();
    }

    public async Task<List<JobApplication>>
        GetApplicationsByUserIdAsync(Guid userId)
    {
        return await _context.JobApplications
            .Where(application =>
                application.JobSeekerUserId == userId)
            .ToListAsync();
    }
}