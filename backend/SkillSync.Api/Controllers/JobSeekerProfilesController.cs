using Microsoft.AspNetCore.Mvc;
using SkillSync.Api.DTOs.JobSeeker;
using SkillSync.Api.Services.Interfaces;

namespace SkillSync.Api.Controllers;

[ApiController]
[Route("api/job-seeker-profiles")]
public class JobSeekerProfilesController : ControllerBase
{
    private readonly IJobSeekerProfileService _service;

    public JobSeekerProfilesController(
        IJobSeekerProfileService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<JobSeekerProfileDto>>> GetAll()
    {
        var profiles = await _service.GetAllAsync();

        return Ok(profiles);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<JobSeekerProfileDto>> GetById(
        Guid id)
    {
        var profile = await _service.GetByIdAsync(id);

        if (profile is null)
        {
            return NotFound();
        }

        return Ok(profile);
    }

    [HttpGet("user/{userId:guid}")]
    public async Task<ActionResult<JobSeekerProfileDto>> GetByUserId(
        Guid userId)
    {
        var profile = await _service.GetByUserIdAsync(userId);

        if (profile is null)
        {
            return NotFound();
        }

        return Ok(profile);
    }

    [HttpPost]
    public async Task<ActionResult<JobSeekerProfileDto>> Create(
        CreateJobSeekerProfileDto dto)
    {
        var profile = await _service.CreateAsync(dto);

        if (profile is null)
        {
            return Conflict(
                new { message = "This user already has a profile." });
        }

        return CreatedAtAction(
            nameof(GetById),
            new { id = profile.Id },
            profile);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        UpdateJobSeekerProfileDto dto)
    {
        var updated = await _service.UpdateAsync(id, dto);

        if (!updated)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _service.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}