using GameStore.Api.DTOs.Auth;
using GameStore.Api.Services.Auth;
using GameStore.Data.Constants;
using GameStore.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using GameStore.Api.Exceptions;
namespace GameStore.Api.Controllers;

[ApiController]
[Route("api/Auth")]
public sealed class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
 
    private readonly SignInManager<ApplicationUser> _signInManager;

    private readonly IJwtTokenService _jwtTokenService;

    private readonly ILogger<AuthController> _logger;

    public AuthController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IJwtTokenService jwtTokenService,
        ILogger<AuthController> logger
    )
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtTokenService = jwtTokenService;
        _logger = logger;
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
            throw new ConflictException(
                "Email already exists.");
        }
        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email
        };

        _logger.LogInformation(
            "Attempting to register user: {Email}",
            request.Email);

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            _logger.LogWarning(
                "Failed to register user: {Email}",
                request.Email);

            return BadRequest(new ProblemDetails
            {
                Title = "Registration Failed",
                Detail = string.Join(
                    ", ",
                    result.Errors.Select(e => e.Description)),
                Status = StatusCodes.Status400BadRequest
            });
        }
        await _userManager.AddToRoleAsync(
            user,
            Roles.User);

        _logger.LogInformation(
            "User registered successfully: {Email}",
            user.Email);

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
        _logger.LogInformation(
            "Attempting to login user: {Email}",
            request.Email);

        var result =
            await _signInManager.CheckPasswordSignInAsync(
                user,
                request.Password,
                lockoutOnFailure: true);

        if (result.IsLockedOut)
        {   
            _logger.LogWarning(
                "User account locked out: {Email}",
                request.Email);
            return Unauthorized(new
            {
                message =
                    "Account locked due to multiple failed attempts."
            });
        }


        if (!result.Succeeded)
        {   
            _logger.LogWarning(
                "Invalid login attempt for user: {Email}",
                request.Email);
            return Unauthorized(new
            {
                message = "Invalid credentials."
            });
        }

        _logger.LogInformation(
            "User logged in successfully: {Email}",
            request.Email);
        var tokens =
            await _jwtTokenService.CreateTokensAsync(user);

        return Ok(tokens);
    }

    // <summary>
    // Logout endpoint placeholder.
    // </summary>
    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Logout(
        RefreshTokenRequestDto request
    )
    {
        await _jwtTokenService
        .RevokeRefreshTokenAsync(
            request.RefreshToken);
        _logger.LogInformation(
            "User logged out and refresh token revoked.");
        return NoContent();
    }

    // <summary>
    // Refresh expired JWT access token
    // </summary>
    [HttpPost("refresh")]
    [ProducesResponseType(typeof(AuthResponseDto),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Refresh(
        RefreshTokenRequestDto request)
    {
        var tokens =
            await _jwtTokenService.RefreshTokenAsync(request.RefreshToken);

        if (tokens is null)
        {
            return Unauthorized(new
            {
                message = "Invalid refresh token."
            });
        }
        return Ok(tokens);
    }
}