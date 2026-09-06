using SkillSync.Api.DTOs.JobSeeker;

namespace SkillSync.Api.Services.Interfaces;

public interface IJobSeekerCvService
{
    Task<JobSeekerCvDto?> GetByUserIdAsync(Guid userId);
    Task<(JobSeekerCvDto? Cv, string? Error)> UploadAsync(Guid userId, IFormFile file);
    Task<(byte[] Bytes, string ContentType, string FileName)?> DownloadAsync(Guid userId);
    Task<bool> DeleteAsync(Guid userId);
}
