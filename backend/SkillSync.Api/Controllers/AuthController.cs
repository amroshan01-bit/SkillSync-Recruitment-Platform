using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillSync.Api.DTOs.Auth;
using SkillSync.Api.Services.Interfaces;

namespace SkillSync.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register(
        RegisterRequestDto request)
    {
        var result = await _authService.RegisterAsync(request);

        if (result is null)
        {
            return BadRequest(new
            {
                message =
                    "Email already exists or role is invalid."
            });
        }

        return Ok(result);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login(
        LoginRequestDto request)
    {
        var result = await _authService.LoginAsync(request);

        if (result is null)
        {
            return Unauthorized(new
            {
                message = "Invalid email or password."
            });
        }

        return Ok(result);
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult GetCurrentUser()
    {
        var userId =
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? User.FindFirst(
                JwtRegisteredClaimNames.Sub)?.Value;

        var email =
            User.FindFirst(ClaimTypes.Email)?.Value
            ?? User.FindFirst(
                JwtRegisteredClaimNames.Email)?.Value;

        var fullName =
            User.FindFirst(ClaimTypes.Name)?.Value;

        var role =
            User.FindFirst(ClaimTypes.Role)?.Value;

        return Ok(new
        {
            userId,
            fullName,
            email,
            role
        });
    }
}