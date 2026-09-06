using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkillSync.Api.Models;

public class Vacancy
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public Guid EmployerProfileId { get; set; }

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

    [Column(TypeName = "decimal(18,2)")]
    public decimal MinimumSalary { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal MaximumSalary { get; set; }

    [Required]
    [MaxLength(10)]
    public string Currency { get; set; } = "LKR";

    public DateTime ApplicationClosingDate { get; set; }

    [Range(1, 1000)]
    public int NumberOfOpenings { get; set; } = 1;

    [Required]
    [MaxLength(20)]
    public string Status { get; set; } = "Draft";

    public DateTime? PostedAtUtc { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;

    public EmployerProfile? EmployerProfile { get; set; }
}