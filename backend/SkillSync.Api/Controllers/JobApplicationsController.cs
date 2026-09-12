using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillSync.Api.DTOs.Application;
using SkillSync.Api.Models;
using SkillSync.Api.Services.Interfaces;

namespace SkillSync.Api.Controllers;

[ApiController]
[Route("api/job-applications")]
public class JobApplicationsController : ControllerBase
{
    private readonly IJobApplicationService _applicationService;

    public JobApplicationsController(
        IJobApplicationService applicationService)
    {
        _applicationService = applicationService;
    }

    [HttpPost]
    [Authorize(Roles = UserRoles.JobSeeker)]
    public async Task<ActionResult<JobApplicationDto>> Create(
        CreateJobApplicationRequest request)
    {
        if (!TryGetCurrentUserId(out var jobSeekerUserId))
        {
            return Unauthorized();
        }

        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var result =
            await _applicationService.CreateAsync(
                jobSeekerUserId,
                request);

        if (result.Application is null)
        {
            if (result.ErrorMessage?.Contains(
                    "already",
                    StringComparison.OrdinalIgnoreCase) == true)
            {
                return Conflict(new
                {
                    message = result.ErrorMessage
                });
            }

            return BadRequest(new
            {
                message = result.ErrorMessage
            });
        }

        return StatusCode(
            StatusCodes.Status201Created,
            result.Application);
    }

    [HttpGet("job-seeker/me")]
    [Authorize(Roles = UserRoles.JobSeeker)]
    public async Task<ActionResult<List<JobApplicationDto>>>
        GetMyApplications()
    {
        if (!TryGetCurrentUserId(out var jobSeekerUserId))
        {
            return Unauthorized();
        }

        var applications =
            await _applicationService
                .GetByJobSeekerUserIdAsync(
                    jobSeekerUserId);

        return Ok(applications);
    }

    [HttpGet("vacancy/{vacancyId:guid}")]
    [Authorize(Roles = UserRoles.Employer)]
    public async Task<ActionResult<List<JobApplicationDto>>>
        GetByVacancy(Guid vacancyId)
    {
        if (!TryGetCurrentUserId(out var employerUserId))
        {
            return Unauthorized();
        }

        if (vacancyId == Guid.Empty)
        {
            return BadRequest(
                "A valid vacancy ID is required.");
        }

        var applications =
            await _applicationService.GetByVacancyIdAsync(
                employerUserId,
                vacancyId);

        if (applications is null)
        {
            return NotFound(
                "Vacancy was not found or does not belong to the logged-in employer.");
        }

        return Ok(applications);
    }

    [HttpGet("vacancy/{vacancyId:guid}/ranked")]
    [Authorize(Roles = UserRoles.Employer)]
    public async Task<ActionResult<List<RankedApplicantDto>>>
        GetRankedApplicants(Guid vacancyId)
    {
        if (!TryGetCurrentUserId(out var employerUserId))
        {
            return Unauthorized();
        }

        if (vacancyId == Guid.Empty)
        {
            return BadRequest(
                "A valid vacancy ID is required.");
        }

        var applicants =
            await _applicationService.GetRankedApplicantsAsync(
                employerUserId,
                vacancyId);

        if (applicants is null)
        {
            return NotFound(
                "Vacancy was not found or does not belong to the logged-in employer.");
        }

        return Ok(applicants);
    }

    [HttpPatch("{applicationId:guid}/status")]
    [Authorize(Roles = UserRoles.Employer)]
    public async Task<IActionResult> UpdateStatus(
        Guid applicationId,
        UpdateApplicationStatusRequest request)
    {
        if (!TryGetCurrentUserId(out var employerUserId))
        {
            return Unauthorized();
        }

        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var updated =
            await _applicationService.UpdateStatusAsync(
                employerUserId,
                applicationId,
                request);

        if (!updated)
        {
            return BadRequest(
                "Application not found, does not belong to your vacancy, or status is invalid.");
        }

        return NoContent();
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