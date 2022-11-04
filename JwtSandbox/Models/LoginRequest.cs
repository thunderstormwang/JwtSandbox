namespace JwtSandbox.Models;

public class LoginRequest
{
    public string Account { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
}