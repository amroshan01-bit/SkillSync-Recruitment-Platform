namespace SkillSync.Api.DTOs.JobSeeker;

public class JobSeekerProfileDto
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string? ProfessionalTitle { get; set; }

    public string? Bio { get; set; }

    public string? Location { get; set; }

    public string? PhoneNumber { get; set; }

    public int YearsOfExperience { get; set; }

    public string? HighestQualification { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime UpdatedAtUtc { get; set; }
}