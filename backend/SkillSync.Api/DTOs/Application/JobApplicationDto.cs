namespace SkillSync.Api.DTOs.Application;

public class JobApplicationDto
{
    public Guid Id { get; set; }
    public Guid VacancyId { get; set; }
    public Guid JobSeekerUserId { get; set; }
    public string JobTitle { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? CoverLetter { get; set; }
    public DateTime AppliedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }
}
