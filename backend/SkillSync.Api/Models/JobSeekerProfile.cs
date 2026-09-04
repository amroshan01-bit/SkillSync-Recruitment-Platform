using System.ComponentModel.DataAnnotations;

namespace SkillSync.Api.Models;

public class JobSeekerProfile
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid UserId { get; set; }

    [Required]
    [MaxLength(150)]
    public string FullName { get; set; } = string.Empty;

    [MaxLength(150)]
    public string? ProfessionalTitle { get; set; }

    [MaxLength(1000)]
    public string? Bio { get; set; }

    [MaxLength(150)]
    public string? Location { get; set; }

    [MaxLength(30)]
    public string? PhoneNumber { get; set; }

    [Range(0, 60)]
    public int YearsOfExperience { get; set; }

    [MaxLength(200)]
    public string? HighestQualification { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;
}