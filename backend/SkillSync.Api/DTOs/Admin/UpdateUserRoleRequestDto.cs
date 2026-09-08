using System.ComponentModel.DataAnnotations;

namespace SkillSync.Api.DTOs.Admin;

public class UpdateUserRoleRequestDto
{
    [Required]
    public string Role { get; set; } = string.Empty;
}