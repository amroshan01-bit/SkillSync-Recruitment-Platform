using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillSync.Api.DTOs.Admin;
using SkillSync.Api.Models;
using SkillSync.Api.Services.Interfaces;

namespace SkillSync.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = UserRoles.Admin)]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;

    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    [HttpGet("users")]
    public async Task<ActionResult<List<AdminUserDto>>> GetAllUsers()
    {
        var users = await _adminService.GetAllUsersAsync();

        return Ok(users);
    }

    [HttpGet("users/{userId:guid}")]
    public async Task<ActionResult<AdminUserDto>> GetUserById(
        Guid userId)
    {
        var user = await _adminService.GetUserByIdAsync(userId);

        if (user is null)
        {
            return NotFound(new
            {
                message = "User was not found."
            });
        }

        return Ok(user);
    }

    [HttpPut("users/{userId:guid}/role")]
    public async Task<ActionResult<AdminUserDto>> UpdateUserRole(
        Guid userId,
        UpdateUserRoleRequestDto request)
    {
        try
        {
            var user = await _adminService.UpdateUserRoleAsync(
                userId,
                request);

            if (user is null)
            {
                return NotFound(new
                {
                    message = "User was not found."
                });
            }

            return Ok(user);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new
            {
                message = exception.Message
            });
        }
    }

    [HttpPut("users/{userId:guid}/status")]
    public async Task<ActionResult<AdminUserDto>> UpdateUserStatus(
        Guid userId,
        UpdateUserStatusRequestDto request)
    {
        var user = await _adminService.UpdateUserStatusAsync(
            userId,
            request);

        if (user is null)
        {
            return NotFound(new
            {
                message = "User was not found."
            });
        }

        return Ok(user);
    }
}