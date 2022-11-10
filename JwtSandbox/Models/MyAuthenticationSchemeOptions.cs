using Microsoft.AspNetCore.Authentication;

namespace JwtSandbox.Middlewares;

public class MyAuthenticationSchemeOptions: AuthenticationSchemeOptions
{
    public string Hello { get; set; }
}