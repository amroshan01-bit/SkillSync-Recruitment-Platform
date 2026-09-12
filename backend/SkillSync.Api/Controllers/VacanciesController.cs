using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillSync.Api.DTOs.Vacancy;
using SkillSync.Api.Models;
using SkillSync.Api.Services.Interfaces;

namespace SkillSync.Api.Controllers;

[ApiController]
[Route("api/vacancies")]
public class VacanciesController : ControllerBase
{
    private readonly IVacancyService _service;

    public VacanciesController(IVacancyService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<VacancyDto>>> GetAll(
        [FromQuery] string? status,
        [FromQuery] string? search)
    {
        var vacancies =
            await _service.GetAllAsync(status, search);

        return Ok(vacancies);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<VacancyDto>> GetById(Guid id)
    {
        var vacancy = await _service.GetByIdAsync(id);

        return vacancy is null ? NotFound() : Ok(vacancy);
    }

    [HttpGet("mine")]
    [Authorize(Roles = UserRoles.Employer)]
    public async Task<ActionResult<List<VacancyDto>>> GetMine(
        [FromQuery] string? status)
    {
        if (!TryGetCurrentUserId(out var userId))
        {
            return Unauthorized();
        }

        var vacancies =
            await _service.GetCurrentEmployerAsync(
                userId,
                status);

        if (vacancies is null)
        {
            return NotFound(new
            {
                message =
                    "Create an employer profile before managing vacancies."
            });
        }

        return Ok(vacancies);
    }

    [HttpPost]
    [Authorize(Roles = UserRoles.Employer)]
    public async Task<ActionResult<VacancyDto>> Create(
        CreateVacancyDto dto)
    {
        if (!TryGetCurrentUserId(out var userId))
        {
            return Unauthorized();
        }

        var vacancy =
            await _service.CreateAsync(userId, dto);

        if (vacancy is null)
        {
            return BadRequest(new
            {
                message =
                    "Check the employer profile, salary range and closing date."
            });
        }

        return CreatedAtAction(
            nameof(GetById),
            new { id = vacancy.Id },
            vacancy);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = UserRoles.Employer)]
    public async Task<IActionResult> Update(
        Guid id,
        UpdateVacancyDto dto)
    {
        if (!TryGetCurrentUserId(out var userId))
        {
            return Unauthorized();
        }

        var updated =
            await _service.UpdateAsync(userId, id, dto);

        if (!updated)
        {
            return BadRequest(new
            {
                message =
                    "The vacancy cannot be updated. Check its ownership, status and values."
            });
        }

        return NoContent();
    }

    [HttpPatch("{id:guid}/close")]
    [Authorize(Roles = UserRoles.Employer)]
    public async Task<IActionResult> Close(Guid id)
    {
        if (!TryGetCurrentUserId(out var userId))
        {
            return Unauthorized();
        }

        var closed =
            await _service.CloseAsync(userId, id);

        if (!closed)
        {
            return BadRequest(new
            {
                message =
                    "The vacancy was not found, is not yours or is already closed."
            });
        }

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = UserRoles.Employer)]
    public async Task<IActionResult> Delete(Guid id)
    {
        if (!TryGetCurrentUserId(out var userId))
        {
            return Unauthorized();
        }

        var deleted =
            await _service.DeleteAsync(userId, id);

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