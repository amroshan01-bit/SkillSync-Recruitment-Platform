using System.ComponentModel.DataAnnotations;

namespace SkillSync.Api.DTOs.Application;

public class CreateJobApplicationRequest
{
    public Guid VacancyId { get; set; }

    [MaxLength(2000)]
    public string? CoverLetter { get; set; }
}
