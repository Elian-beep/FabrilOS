using System.Security.Claims;
using FabrilOS.API.Entities;

namespace FabrilOS.API.Services.Interfaces;

public interface ITokenService
{
  string GenerateAccessToken(User user);
  string GenerateRefreshToken();
  ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}