using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillSync.Api.DTOs.Matching;
using SkillSync.Api.Models;
using SkillSync.Api.Services.Interfaces;

namespace SkillSync.Api.Controllers;

[ApiController]
[Route("api/matching")]
[Authorize(Roles = UserRoles.JobSeeker)]
public class MatchingController : ControllerBase
{
    private readonly IMatchingService _matchingService;

    public MatchingController(
        IMatchingService matchingService)
    {
        _matchingService = matchingService;
    }

    [HttpGet("skills")]
    public async Task<ActionResult<List<string>>> GetSkills()
    {
        if (!TryGetCurrentUserId(out var userId))
        {
            return Unauthorized();
        }

        var skills =
            await _matchingService.GetSkillsAsync(userId);

        return Ok(skills);
    }

    [HttpPut("skills")]
    public async Task<IActionResult> UpdateSkills(
        UpdateJobSeekerSkillsRequest request)
    {
        if (!TryGetCurrentUserId(out var userId))
        {
            return Unauthorized();
        }

        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var updated =
            await _matchingService.UpdateSkillsAsync(
                userId,
                request);

        if (!updated)
        {
            return BadRequest(
                "The skills could not be updated.");
        }

        return NoContent();
    }

    [HttpGet("results")]
    public async Task<ActionResult<List<MatchResultDto>>>
        GetMatches()
    {
        if (!TryGetCurrentUserId(out var userId))
        {
            return Unauthorized();
        }

        var matches =
            await _matchingService.GetMatchesAsync(userId);

        return Ok(matches);
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