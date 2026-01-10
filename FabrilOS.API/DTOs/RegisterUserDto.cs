using System.ComponentModel.DataAnnotations;

namespace FabrilOS.API.DTOs;

public class RegisterUserDto
{
    [Required(ErrorMessage = "O nome é obrigatório")]
    public string FullName { get; set; }

    [Required(ErrorMessage = "O email é obrigatório")]
    [EmailAddress]
    public string Email { get; set; }

    [Required(ErrorMessage = "A senha é obrigatória")]
    [MinLength(3, ErrorMessage = "A senha deve ter no mínimo 3 caracteres")]
    public string Password { get; set; }
    
    [Compare("Password", ErrorMessage = "As senhas não conferem")]
    public string ConfirmPassword { get; set; }
}