using System.ComponentModel.DataAnnotations;

namespace SkillSync.Api.DTOs.Employer;

public class CreateEmployerProfileDto
{
    [Required]
    [MaxLength(150)]
    public string CompanyName { get; set; } = string.Empty;

    [Required]
    [MaxLength(150)]
    public string Industry { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string CompanySize { get; set; } = string.Empty;

    [MaxLength(250)]
    [Url]
    public string? Website { get; set; }

    [Required]
    [MaxLength(150)]
    public string Location { get; set; } = string.Empty;

    [Required]
    [MaxLength(2000)]
    public string AboutCompany { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? LogoPath { get; set; }

    [Required]
    [MaxLength(150)]
    public string ContactPerson { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    [EmailAddress]
    public string EmailAddress { get; set; } = string.Empty;

    [Required]
    [MaxLength(30)]
    [Phone]
    public string PhoneNumber { get; set; } = string.Empty;
}