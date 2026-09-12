namespace SkillSync.Api.DTOs.Application;

public class RankedApplicantDto
{
    public Guid ApplicationId { get; set; }

    public Guid VacancyId { get; set; }

    public Guid JobSeekerUserId { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string? ProfessionalTitle { get; set; }

    public string? Location { get; set; }

    public int YearsOfExperience { get; set; }

    public string? HighestQualification { get; set; }

    public string Status { get; set; } = string.Empty;

    public string? CoverLetter { get; set; }

    public double MatchScore { get; set; }

    public List<string> MatchedSkills { get; set; } = [];

    public List<string> MissingSkills { get; set; } = [];

    public DateTime AppliedAtUtc { get; set; }
}