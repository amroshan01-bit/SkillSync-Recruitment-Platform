using Microsoft.AspNetCore.Mvc;
using SkillSync.Api.DTOs.Application;
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

    // Create a new job application.
    [HttpPost]
    public async Task<ActionResult<JobApplicationDto>> Create(
        CreateJobApplicationRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var result =
            await _applicationService.CreateAsync(request);

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

        return CreatedAtAction(
            nameof(GetByJobSeeker),
            new
            {
                jobSeekerUserId =
                    result.Application.JobSeekerUserId
            },
            result.Application);
    }

    // Get applications submitted by one job seeker.
    [HttpGet("job-seeker/{jobSeekerUserId:guid}")]
    public async Task<ActionResult<List<JobApplicationDto>>>
        GetByJobSeeker(Guid jobSeekerUserId)
    {
        if (jobSeekerUserId == Guid.Empty)
        {
            return BadRequest(
                "A valid job seeker user ID is required.");
        }

        var applications =
            await _applicationService
                .GetByJobSeekerUserIdAsync(
                    jobSeekerUserId);

        return Ok(applications);
    }

    // Get applications received for one vacancy.
    [HttpGet("vacancy/{vacancyId:guid}")]
    public async Task<ActionResult<List<JobApplicationDto>>>
        GetByVacancy(Guid vacancyId)
    {
        if (vacancyId == Guid.Empty)
        {
            return BadRequest(
                "A valid vacancy ID is required.");
        }

        var applications =
            await _applicationService
                .GetByVacancyIdAsync(vacancyId);

        return Ok(applications);
    }

    // Update an application's current status.
    [HttpPatch("{applicationId:guid}/status")]
    public async Task<IActionResult> UpdateStatus(
        Guid applicationId,
        UpdateApplicationStatusRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var updated =
            await _applicationService.UpdateStatusAsync(
                applicationId,
                request);

        if (!updated)
        {
            return BadRequest(
                "Application not found or status is invalid.");
        }

        return NoContent();
    }
}