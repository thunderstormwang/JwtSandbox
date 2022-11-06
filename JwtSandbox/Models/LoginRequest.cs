using System.ComponentModel.DataAnnotations;

namespace JwtSandbox.Models;

public class LoginRequest
{
    [Required]
    public string? Account { get; set; }
    
    [Required]
    public string? Password { get; set; }
    
    [Required]
    public string? Name { get; set; }
}