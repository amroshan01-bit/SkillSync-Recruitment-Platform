using Microsoft.EntityFrameworkCore;
using SkillSync.Api.Data;
using SkillSync.Api.Models;
using SkillSync.Api.Repositories.Interfaces;

namespace SkillSync.Api.Repositories.Implementations;

public class JobApplicationRepository
    : IJobApplicationRepository
{
    private readonly ApplicationDbContext _context;

    public JobApplicationRepository(
        ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Vacancy?> GetVacancyByIdAsync(
        Guid vacancyId)
    {
        return await _context.Vacancies
            .Include(vacancy => vacancy.EmployerProfile)
            .FirstOrDefaultAsync(vacancy =>
                vacancy.Id == vacancyId);
    }

    public async Task<bool> ApplicationExistsAsync(
        Guid jobSeekerUserId,
        Guid vacancyId)
    {
        return await _context.JobApplications
            .AnyAsync(application =>
                application.JobSeekerUserId ==
                    jobSeekerUserId &&
                application.VacancyId == vacancyId);
    }

    public async Task<JobApplication> CreateAsync(
        JobApplication application)
    {
        _context.JobApplications.Add(application);
        await _context.SaveChangesAsync();

        return application;
    }

    public async Task<List<JobApplication>>
        GetByJobSeekerUserIdAsync(Guid jobSeekerUserId)
    {
        return await _context.JobApplications
            .Include(application => application.Vacancy)
            .ThenInclude(vacancy =>
                vacancy!.EmployerProfile)
            .Where(application =>
                application.JobSeekerUserId ==
                    jobSeekerUserId)
            .OrderByDescending(application =>
                application.AppliedAtUtc)
            .ToListAsync();
    }

    public async Task<List<JobApplication>>
        GetByVacancyIdAsync(Guid vacancyId)
    {
        return await _context.JobApplications
            .Include(application => application.Vacancy)
            .ThenInclude(vacancy =>
                vacancy!.EmployerProfile)
            .Where(application =>
                application.VacancyId == vacancyId)
            .OrderByDescending(application =>
                application.AppliedAtUtc)
            .ToListAsync();
    }

    public async Task<JobApplication?> GetByIdAsync(Guid id)
    {
        return await _context.JobApplications
            .Include(application => application.Vacancy)
            .ThenInclude(vacancy =>
                vacancy!.EmployerProfile)
            .FirstOrDefaultAsync(application =>
                application.Id == id);
    }

    public async Task UpdateAsync(
        JobApplication application)
    {
        _context.JobApplications.Update(application);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateStatusWithNotificationAsync(
        JobApplication application,
        Notification notification)
    {
        await using var transaction =
            await _context.Database.BeginTransactionAsync();

        try
        {
            _context.JobApplications.Update(application);
            _context.Notifications.Add(notification);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}