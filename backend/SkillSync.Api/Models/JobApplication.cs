using System.ComponentModel.DataAnnotations;

namespace SkillSync.Api.Models;

public class JobApplication
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid VacancyId { get; set; }

    public Guid JobSeekerUserId { get; set; }

    [MaxLength(2000)]
    public string? CoverLetter { get; set; }

    [Required]
    [MaxLength(30)]
    public string Status { get; set; } = "Pending";

    public DateTime AppliedAtUtc { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;

    public Vacancy? Vacancy { get; set; }
}