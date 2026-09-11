using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillSync.Api.DTOs.JobSeeker;
using SkillSync.Api.Models;
using SkillSync.Api.Services.Interfaces;

namespace SkillSync.Api.Controllers;

[ApiController]
[Route("api/job-seeker-cvs")]
[Authorize(Roles = UserRoles.JobSeeker)]
public class JobSeekerCvsController : ControllerBase
{
    private readonly IJobSeekerCvService _service;

    public JobSeekerCvsController(
        IJobSeekerCvService service)
    {
        _service = service;
    }

    [HttpGet("me")]
    public async Task<ActionResult<JobSeekerCvDto>>
        GetCurrent()
    {
        if (!TryGetCurrentUserId(out var userId))
        {
            return Unauthorized();
        }

        var cv = await _service.GetByUserIdAsync(userId);

        return cv is null ? NotFound() : Ok(cv);
    }

    [HttpPost("me/upload")]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<JobSeekerCvDto>>
        UploadCurrent(IFormFile file)
    {
        if (!TryGetCurrentUserId(out var userId))
        {
            return Unauthorized();
        }

        var result =
            await _service.UploadAsync(userId, file);

        return result.Error is not null
            ? BadRequest(new { message = result.Error })
            : Ok(result.Cv);
    }

    [HttpGet("me/download")]
    public async Task<IActionResult> DownloadCurrent()
    {
        if (!TryGetCurrentUserId(out var userId))
        {
            return Unauthorized();
        }

        var file = await _service.DownloadAsync(userId);

        return file is null
            ? NotFound()
            : File(
                file.Value.Bytes,
                file.Value.ContentType,
                file.Value.FileName);
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