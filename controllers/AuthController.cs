using salacerta_auth_service.DTOs;
using Microsoft.AspNetCore.Mvc;
using salacerta_auth_service.Services;

namespace salacerta_auth_service.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDTO dto)
    {
        try
        {
            await _authService.RegisterAsync(dto);

            return StatusCode(201, new
            {
                message = "Usuário criado com sucesso."
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                error = ex.Message
            });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDTO dto)
    {
        try
        {
            var response = await _authService.LoginAsync(dto);

            return Ok(response);
        }
        catch (Exception ex)
        {
            return Unauthorized(new
            {
                error = ex.Message
            });
        }
    }
}