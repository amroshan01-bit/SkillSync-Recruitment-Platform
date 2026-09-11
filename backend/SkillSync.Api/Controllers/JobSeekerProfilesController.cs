using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillSync.Api.DTOs.JobSeeker;
using SkillSync.Api.Models;
using SkillSync.Api.Services.Interfaces;

namespace SkillSync.Api.Controllers;

[ApiController]
[Route("api/job-seeker-profiles")]
[Authorize(Roles = UserRoles.JobSeeker)]
public class JobSeekerProfilesController : ControllerBase
{
    private readonly IJobSeekerProfileService _service;

    public JobSeekerProfilesController(
        IJobSeekerProfileService service)
    {
        _service = service;
    }

    [HttpGet("me")]
    public async Task<ActionResult<JobSeekerProfileDto>>
        GetCurrent()
    {
        if (!TryGetCurrentUserId(out var userId))
        {
            return Unauthorized();
        }

        var profile =
            await _service.GetByUserIdAsync(userId);

        return profile is null ? NotFound() : Ok(profile);
    }

    [HttpPost("me")]
    public async Task<ActionResult<JobSeekerProfileDto>>
        CreateCurrent(CreateJobSeekerProfileDto dto)
    {
        if (!TryGetCurrentUserId(out var userId))
        {
            return Unauthorized();
        }

        var profile =
            await _service.CreateAsync(userId, dto);

        if (profile is null)
        {
            return Conflict(new
            {
                message = "This user already has a profile."
            });
        }

        return CreatedAtAction(
            nameof(GetCurrent),
            profile);
    }

    [HttpPut("me")]
    public async Task<IActionResult> UpdateCurrent(
        UpdateJobSeekerProfileDto dto)
    {
        if (!TryGetCurrentUserId(out var userId))
        {
            return Unauthorized();
        }

        var updated =
            await _service.UpdateAsync(userId, dto);

        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("me")]
    public async Task<IActionResult> DeleteCurrent()
    {
        if (!TryGetCurrentUserId(out var userId))
        {
            return Unauthorized();
        }

        var deleted = await _service.DeleteAsync(userId);

        return deleted ? NoContent() : NotFound();
    }

    private bool TryGetCurrentUserId(out Guid userId)
    {
        var userIdValue =
            User.FindFirst(
                ClaimTypes.NameIdentifier)?.Value
            ?? User.FindFirst(
                JwtRegisteredClaimNames.Sub)?.Value;

        return Guid.TryParse(userIdValue, out userId);
    }
}