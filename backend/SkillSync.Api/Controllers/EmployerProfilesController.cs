using Microsoft.AspNetCore.Mvc;
using SkillSync.Api.DTOs.Employer;
using SkillSync.Api.Services.Interfaces;

namespace SkillSync.Api.Controllers;

[ApiController]
[Route("api/employer-profiles")]
public class EmployerProfilesController : ControllerBase
{
    private readonly IEmployerProfileService _service;

    public EmployerProfilesController(
        IEmployerProfileService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<EmployerProfileDto>>> GetAll()
    {
        var profiles = await _service.GetAllAsync();

        return Ok(profiles);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<EmployerProfileDto>> GetById(
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
    public async Task<ActionResult<EmployerProfileDto>> GetByUserId(
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
    public async Task<ActionResult<EmployerProfileDto>> Create(
        CreateEmployerProfileDto dto)
    {
        var profile = await _service.CreateAsync(dto);

        if (profile is null)
        {
            return Conflict(new
            {
                message = "This user already has an employer profile."
            });
        }

        return CreatedAtAction(
            nameof(GetById),
            new { id = profile.Id },
            profile);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        UpdateEmployerProfileDto dto)
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