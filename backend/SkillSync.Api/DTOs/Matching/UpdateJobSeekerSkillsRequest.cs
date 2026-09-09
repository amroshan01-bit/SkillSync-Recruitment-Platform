using System.ComponentModel.DataAnnotations;

namespace SkillSync.Api.DTOs.Matching;

public class UpdateJobSeekerSkillsRequest
{
    public Guid UserId { get; set; }

    [Required]
    public List<string> Skills { get; set; } = [];
}
