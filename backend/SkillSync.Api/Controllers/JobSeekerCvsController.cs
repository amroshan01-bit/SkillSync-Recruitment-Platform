using Microsoft.AspNetCore.Mvc;
using SkillSync.Api.DTOs.JobSeeker;
using SkillSync.Api.Services.Interfaces;

namespace SkillSync.Api.Controllers;

[ApiController]
[Route("api/job-seeker-cvs")]
public class JobSeekerCvsController : ControllerBase
{
    private readonly IJobSeekerCvService _service;

    public JobSeekerCvsController(IJobSeekerCvService service)
    {
        _service = service;
    }

    [HttpGet("user/{userId:guid}")]
    public async Task<ActionResult<JobSeekerCvDto>> Get(Guid userId)
    {
        var cv = await _service.GetByUserIdAsync(userId);
        return cv is null ? NotFound() : Ok(cv);
    }

    [HttpPost("upload/{userId:guid}")]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<JobSeekerCvDto>> Upload(Guid userId, IFormFile file)
    {
        var result = await _service.UploadAsync(userId, file);
        return result.Error is not null ? BadRequest(new { message = result.Error }) : Ok(result.Cv);
    }

    [HttpGet("download/{userId:guid}")]
    public async Task<IActionResult> Download(Guid userId)
    {
        var file = await _service.DownloadAsync(userId);
        return file is null ? NotFound() : File(file.Value.Bytes, file.Value.ContentType, file.Value.FileName);
    }

    [HttpDelete("user/{userId:guid}")]
    public async Task<IActionResult> Delete(Guid userId)
    {
        return await _service.DeleteAsync(userId) ? NoContent() : NotFound();
    }
}
