namespace SkillSync.Api.DTOs.Employer;

public class EmployerProfileDto
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string CompanyName { get; set; } = string.Empty;

    public string Industry { get; set; } = string.Empty;

    public string CompanySize { get; set; } = string.Empty;

    public string? Website { get; set; }

    public string Location { get; set; } = string.Empty;

    public string AboutCompany { get; set; } = string.Empty;

    public string? LogoPath { get; set; }

    public string ContactPerson { get; set; } = string.Empty;

    public string EmailAddress { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public DateTime CreatedAtUtc { get; set; }

    public DateTime UpdatedAtUtc { get; set; }
}