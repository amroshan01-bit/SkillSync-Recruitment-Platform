using System.ComponentModel.DataAnnotations;

namespace SkillSync.Api.DTOs.Matching;

public class UpdateJobSeekerSkillsRequest
{
    [Required]
    public List<string> Skills { get; set; } = [];
}