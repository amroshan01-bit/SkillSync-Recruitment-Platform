namespace SkillSync.Api.DTOs.Matching;

public class MatchResultDto
{
    public Guid VacancyId { get; set; }
    public string JobTitle { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string WorkplaceType { get; set; } = string.Empty;
    public string EmploymentType { get; set; } = string.Empty;
    public decimal MinimumSalary { get; set; }
    public decimal MaximumSalary { get; set; }
    public string Currency { get; set; } = string.Empty;
    public double MatchScore { get; set; }
    public List<string> MatchedSkills { get; set; } = [];
    public List<string> MissingSkills { get; set; } = [];
    public bool HasApplied { get; set; }
    public string? ApplicationStatus { get; set; }
}
