using Microsoft.AspNetCore.Mvc;
using SkillSync.Api.DTOs.Vacancy;
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

        if (vacancy is null)
        {
            return NotFound();
        }

        return Ok(vacancy);
    }

    [HttpGet("employer/{employerProfileId:guid}")]
    public async Task<ActionResult<List<VacancyDto>>> GetByEmployer(
        Guid employerProfileId,
        [FromQuery] string? status)
    {
        var vacancies =
            await _service.GetByEmployerProfileIdAsync(
                employerProfileId,
                status);

        return Ok(vacancies);
    }

    [HttpPost]
    public async Task<ActionResult<VacancyDto>> Create(
        CreateVacancyDto dto)
    {
        var vacancy = await _service.CreateAsync(dto);

        if (vacancy is null)
        {
            return BadRequest(new
            {
                message =
                    "Check the employer, salary range and closing date."
            });
        }

        return CreatedAtAction(
            nameof(GetById),
            new { id = vacancy.Id },
            vacancy);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        UpdateVacancyDto dto)
    {
        var updated = await _service.UpdateAsync(id, dto);

        if (!updated)
        {
            return BadRequest(new
            {
                message =
                    "The vacancy cannot be updated. Check its status and values."
            });
        }

        return NoContent();
    }

    [HttpPatch("{id:guid}/close")]
    public async Task<IActionResult> Close(Guid id)
    {
        var closed = await _service.CloseAsync(id);

        if (!closed)
        {
            return BadRequest(new
            {
                message =
                    "The vacancy was not found or is already closed."
            });
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