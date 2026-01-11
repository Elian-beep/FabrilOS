using FabrilOS.API.Data;
using FabrilOS.API.DTOs;
using FabrilOS.API.Entities;
using FabrilOS.API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FabrilOS.API.Services.Implementations;

public class AuthService : IAuthService
{
  private readonly FabrilOSContext _context;
  private readonly ITokenService _tokenService;
  private readonly IPasswordHasher<User> _passwordHasher;
  private readonly IConfiguration _configuration;

  public AuthService(
        FabrilOSContext context,
        ITokenService tokenService,
        IPasswordHasher<User> passwordHasher,
        IConfiguration configuration)
  {
    _context = context;
    _tokenService = tokenService;
    _passwordHasher = passwordHasher;
    _configuration = configuration;
  }

  public async Task<string?> RegisterAsync(RegisterUserDto dto)
  {
    if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
    {
      return "Este email já está em uso.";
    }

    var user = new User
    {
      Name = dto.FullName,
      Email = dto.Email
    };

    user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);

    _context.Users.Add(user);
    await _context.SaveChangesAsync();

    return null;
  }

  public async Task<TokenResponseDto?> LoginAsync(LoginUserDto dto)
  {
    var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);

    if (user == null) return null;

    var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);

    if (result == PasswordVerificationResult.Failed)
      return null;

    var accessToken = _tokenService.GenerateAccessToken(user);
    var refreshToken = _tokenService.GenerateRefreshToken();

    _ = int.TryParse(_configuration["Jwt:RefreshTokenExpireDays"], out int refreshExpireDays);
    user.RefreshToken = refreshToken;
    user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(refreshExpireDays);

    await _context.SaveChangesAsync();

    return new TokenResponseDto
    {
      AccessToken = accessToken,
      RefreshToken = refreshToken,
      UserName = user.Name,
      Email = user.Email
    };
  }

  public async Task<TokenResponseDto?> RefreshTokenAsync(string accessToken, string refreshToken)
  {
    var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
    var userEmail = principal.FindFirst(ClaimTypes.Email)?.Value;

    if (userEmail == null) return null;

    var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);

    if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
    {
      return null;
    }

    var newAccessToken = _tokenService.GenerateAccessToken(user);
    var newRefreshToken = _tokenService.GenerateRefreshToken();

    user.RefreshToken = newRefreshToken;
    await _context.SaveChangesAsync();

    return new TokenResponseDto
    {
      AccessToken = newAccessToken,
      RefreshToken = newRefreshToken,
      UserName = user.Name,
      Email = user.Email
    };
  }
}