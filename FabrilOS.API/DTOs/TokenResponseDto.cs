namespace FabrilOS.API.DTOs;

public class TokenResponseDto
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public string UserName { get; set; } 
    public string Email { get; set; }
}