using Microsoft.AspNetCore.Mvc;
using SkillSync.Api.DTOs.Matching;
using SkillSync.Api.Services.Interfaces;

namespace SkillSync.Api.Controllers;

[ApiController]
[Route("api/matching")]
public class MatchingController : ControllerBase
{
    private readonly IMatchingService _matchingService;

    public MatchingController(
        IMatchingService matchingService)
    {
        _matchingService = matchingService;
    }

    // Get the job seeker's saved skills.
    [HttpGet("skills/{userId:guid}")]
    public async Task<ActionResult<List<string>>> GetSkills(
        Guid userId)
    {
        if (userId == Guid.Empty)
        {
            return BadRequest("A valid user ID is required.");
        }

        var skills =
            await _matchingService.GetSkillsAsync(userId);

        return Ok(skills);
    }

    // Replace the job seeker's current skill list.
    [HttpPut("skills")]
    public async Task<IActionResult> UpdateSkills(
        UpdateJobSeekerSkillsRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var updated =
            await _matchingService.UpdateSkillsAsync(request);

        if (!updated)
        {
            return BadRequest(
                "The skills could not be updated.");
        }

        return NoContent();
    }

    // Get matching jobs with backend-calculated scores.
    [HttpGet("results/{userId:guid}")]
    public async Task<ActionResult<List<MatchResultDto>>>
        GetMatches(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            return BadRequest("A valid user ID is required.");
        }

        var matches =
            await _matchingService.GetMatchesAsync(userId);

        return Ok(matches);
    }
}