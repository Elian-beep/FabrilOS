using FabrilOS.API.DTOs;
using FabrilOS.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FabrilOS.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
  private readonly IAuthService _authService;

  public AuthController(IAuthService authService)
  {
    _authService = authService;
  }

  [HttpPost("register")]
  public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
  {
    var errorMessage = await _authService.RegisterAsync(dto);
    
    if (errorMessage != null)
    {
      return BadRequest(errorMessage);
    }
    

    return Ok("Usuário registrado com sucesso!");
  }

  [HttpPost("login")]
  public async Task<IActionResult> Login([FromBody] LoginUserDto dto)
  {
    var tokenResult = await _authService.LoginAsync(dto);

    if (tokenResult == null)
    {
      return Unauthorized("Email ou senha inválidos.");
    }

    return Ok(tokenResult);
  }

  [HttpPost("refresh-token")]
  public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto dto)
  {
    var tokenResult = await _authService.RefreshTokenAsync(dto.AccessToken, dto.RefreshToken);

    if (tokenResult == null)
    {
      return BadRequest("Token inválido ou expirado. Faça login novamente.");
    }

    return Ok(tokenResult);
  }

  [HttpGet("test-auth")]
  [Authorize]
  public IActionResult TestAuth()
  {
    return Ok($"Autenticado com sucesso. Usuário: {User.Identity?.Name}");
  }
}