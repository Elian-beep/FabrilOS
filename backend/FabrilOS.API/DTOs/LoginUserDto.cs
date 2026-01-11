using System.ComponentModel.DataAnnotations;

namespace FabrilOS.API.DTOs;

public class LoginUserDto
{
    [Required(ErrorMessage = "O email é obrigatório")]
    [EmailAddress]
    public string Email { get; set; }

    [Required(ErrorMessage = "A senha é obrigatória")]
    public string Password { get; set; }
}