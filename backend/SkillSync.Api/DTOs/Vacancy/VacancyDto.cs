namespace SkillSync.Api.DTOs.Vacancy;

public class VacancyDto
{
    public Guid Id { get; set; }

    public Guid EmployerProfileId { get; set; }

    public string CompanyName { get; set; } = string.Empty;

    public string JobTitle { get; set; } = string.Empty;

    public string Department { get; set; } = string.Empty;

    public string Location { get; set; } = string.Empty;

    public string WorkplaceType { get; set; } = string.Empty;

    public string EmploymentType { get; set; } = string.Empty;

    public string ExperienceLevel { get; set; } = string.Empty;

    public string JobDescription { get; set; } = string.Empty;

    public string Requirements { get; set; } = string.Empty;

    public string RequiredSkills { get; set; } = string.Empty;

    public decimal MinimumSalary { get; set; }

    public decimal MaximumSalary { get; set; }

    public string Currency { get; set; } = string.Empty;

    public DateTime ApplicationClosingDate { get; set; }

    public int NumberOfOpenings { get; set; }

    public string Status { get; set; } = string.Empty;

    public DateTime? PostedAtUtc { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime UpdatedAtUtc { get; set; }
}