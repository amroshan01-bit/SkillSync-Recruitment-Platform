using System.ComponentModel.DataAnnotations;

namespace SkillSync.Api.DTOs.Application;

public class UpdateApplicationStatusRequest
{
    [Required]
    [MaxLength(30)]
    public string Status { get; set; } = string.Empty;
}
