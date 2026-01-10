using FabrilOS.API.DTOs;
using Microsoft.AspNetCore.Identity;

namespace FabrilOS.API.Services.Interfaces;

public interface IAuthService
{
  Task<string?> RegisterAsync(RegisterUserDto dto);
  Task<TokenResponseDto?> LoginAsync(LoginUserDto dto);
  Task<TokenResponseDto?> RefreshTokenAsync(string accessToken, string refreshToken);
}