using Backend.Models.DTOs.Auth;
using Backend.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterDto model)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterAsync(model);
            if (!result)
                return BadRequest("Registration failed. Please check your information and try again.");

            return Ok(new { message = "Registration successful" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during registration for user {Email}", model.Email);
            return StatusCode(500, "An error occurred during registration.");
        }
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginDto model)
    {
        try
        {
            var token = await _authService.LoginAsync(model);
            return Ok(new { token, message = "Login successful" });
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized(new { message = "Invalid email or password" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for user {Email}", model.Email);
            return StatusCode(500, "An error occurred during login.");
        }
    }
}