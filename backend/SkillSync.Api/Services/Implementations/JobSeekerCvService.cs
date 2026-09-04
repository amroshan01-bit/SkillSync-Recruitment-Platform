using SkillSync.Api.DTOs.JobSeeker;
using SkillSync.Api.Models;
using SkillSync.Api.Repositories.Interfaces;
using SkillSync.Api.Services.Interfaces;

namespace SkillSync.Api.Services.Implementations;

public class JobSeekerCvService : IJobSeekerCvService
{
    private const long MaximumFileSize = 5 * 1024 * 1024;
    private static readonly string[] AllowedExtensions = [".pdf", ".doc", ".docx"];
    private readonly IJobSeekerCvRepository _repository;
    private readonly IWebHostEnvironment _environment;

    public JobSeekerCvService(IJobSeekerCvRepository repository, IWebHostEnvironment environment)
    {
        _repository = repository;
        _environment = environment;
    }

    // Returns the CV metadata for one user.
    public async Task<JobSeekerCvDto?> GetByUserIdAsync(Guid userId)
    {
        var cv = await _repository.GetByUserIdAsync(userId);
        return cv is null ? null : MapToDto(cv);
    }

    // Validates and stores one PDF/DOC/DOCX file. A new upload replaces the old CV.
    public async Task<(JobSeekerCvDto? Cv, string? Error)> UploadAsync(Guid userId, IFormFile file)
    {
        if (file.Length == 0)
            return (null, "Please select a non-empty CV file.");

        if (file.Length > MaximumFileSize)
            return (null, "The CV file must not exceed 5 MB.");

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!AllowedExtensions.Contains(extension))
            return (null, "Only PDF, DOC and DOCX files are allowed.");

        var uploadDirectory = Path.Combine(_environment.ContentRootPath, "Uploads", "Cvs");
        Directory.CreateDirectory(uploadDirectory);

        var storedFileName = $"{Guid.NewGuid()}{extension}";
        var absolutePath = Path.Combine(uploadDirectory, storedFileName);

        await using (var stream = File.Create(absolutePath))
        {
            await file.CopyToAsync(stream);
        }

        var existingCv = await _repository.GetByUserIdAsync(userId);
        if (existingCv is not null)
        {
            DeletePhysicalFile(existingCv.RelativePath);
            await _repository.DeleteAsync(existingCv);
        }

        var cv = new JobSeekerCv
        {
            UserId = userId,
            OriginalFileName = Path.GetFileName(file.FileName),
            StoredFileName = storedFileName,
            ContentType = GetContentType(extension),
            FileSize = file.Length,
            RelativePath = Path.Combine("Uploads", "Cvs", storedFileName)
        };

        await _repository.CreateAsync(cv);
        return (MapToDto(cv), null);
    }

    // Reads the stored file so the controller can return it as a download.
    public async Task<(byte[] Bytes, string ContentType, string FileName)?> DownloadAsync(Guid userId)
    {
        var cv = await _repository.GetByUserIdAsync(userId);
        if (cv is null) return null;

        var absolutePath = Path.Combine(_environment.ContentRootPath, cv.RelativePath);
        if (!File.Exists(absolutePath)) return null;

        return (await File.ReadAllBytesAsync(absolutePath), cv.ContentType, cv.OriginalFileName);
    }

    // Deletes both the database row and its physical file.
    public async Task<bool> DeleteAsync(Guid userId)
    {
        var cv = await _repository.GetByUserIdAsync(userId);
        if (cv is null) return false;

        DeletePhysicalFile(cv.RelativePath);
        await _repository.DeleteAsync(cv);
        return true;
    }

    private void DeletePhysicalFile(string relativePath)
    {
        var absolutePath = Path.Combine(_environment.ContentRootPath, relativePath);
        if (File.Exists(absolutePath)) File.Delete(absolutePath);
    }

    private static string GetContentType(string extension) => extension switch
    {
        ".pdf" => "application/pdf",
        ".doc" => "application/msword",
        ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
        _ => "application/octet-stream"
    };

    private static JobSeekerCvDto MapToDto(JobSeekerCv cv) => new()
    {
        Id = cv.Id,
        UserId = cv.UserId,
        FileName = cv.OriginalFileName,
        ContentType = cv.ContentType,
        FileSize = cv.FileSize,
        UploadedAtUtc = cv.UploadedAtUtc
    };
}
