using System.ComponentModel.DataAnnotations;

namespace SkillSync.Api.DTOs.Vacancy;

public class UpdateVacancyDto
{
    [Required]
    [MaxLength(150)]
    public string JobTitle { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Department { get; set; } = string.Empty;

    [Required]
    [MaxLength(150)]
    public string Location { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string WorkplaceType { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string EmploymentType { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string ExperienceLevel { get; set; } = string.Empty;

    [Required]
    [MaxLength(5000)]
    public string JobDescription { get; set; } = string.Empty;

    [Required]
    [MaxLength(5000)]
    public string Requirements { get; set; } = string.Empty;

    [Required]
    [MaxLength(1000)]
    public string RequiredSkills { get; set; } = string.Empty;

    [Range(0, double.MaxValue)]
    public decimal MinimumSalary { get; set; }

    [Range(0, double.MaxValue)]
    public decimal MaximumSalary { get; set; }

    [Required]
    [MaxLength(10)]
    public string Currency { get; set; } = "LKR";

    [Required]
    public DateTime ApplicationClosingDate { get; set; }

    [Range(1, 1000)]
    public int NumberOfOpenings { get; set; } = 1;

    [Required]
    [RegularExpression(
        "^(Draft|Open)$",
        ErrorMessage = "Status must be Draft or Open.")]
    public string Status { get; set; } = "Draft";
}