using GameStore.Api.DTOs.Auth;
using GameStore.Api.Services.Auth;
using GameStore.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Api.Controllers;

[ApiController]
[Route("api/Auth")]
public sealed class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;

    private readonly SignInManager<ApplicationUser> _signInManager;

    private readonly IJwtTokenService _jwtTokenService;

    public AuthController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IJwtTokenService jwtTokenService
    )
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtTokenService = jwtTokenService;
    }
    // <summary>
    // Register a new user account.
    // </summary>

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register(
        RegisterRequestDto request
    )
    {
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser is not null)
        {
            return Conflict(new
            {
                message = "Email already exists."
            });
        }
        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            return BadRequest(new
            {
                message = "Failed to create user."
            });
        }

        return CreatedAtAction(nameof(Register), new { email = user.Email }, user);
    }

    // <summary>
    // Login user and return JWT tokens.
    // </summary>
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
     public async Task<IActionResult> Login(
        LoginRequestDto request)
    {
        var user =
            await _userManager.FindByEmailAsync(request.Email);

        if (user is null)
        {
            return Unauthorized(new
            {
                message = "Invalid credentials."
            });
        }

        var result =
            await _signInManager.CheckPasswordSignInAsync(
                user,
                request.Password,
                lockoutOnFailure: true);

        if (!result.Succeeded)
        {
            return Unauthorized(new
            {
                message = "Invalid credentials."
            });
        }

        var tokens =
            await _jwtTokenService.CreateTokensAsync(user);

        return Ok(tokens);
    }

    // <summary>
    // Logout endpoint placeholder.
    // </summary>
    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult Logout()
    {
        return NoContent();
    }
}