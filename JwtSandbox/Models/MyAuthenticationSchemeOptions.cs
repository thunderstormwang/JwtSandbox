using Microsoft.AspNetCore.Authentication;

namespace JwtSandbox.Models;

public class MyAuthenticationSchemeOptions: AuthenticationSchemeOptions
{
    public string Hello { get; set; }
}