using JwtSandbox.Helpers;
using JwtSandbox.Models;
using Microsoft.AspNetCore.Mvc;

namespace JwtSandbox.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : Controller
{
    private readonly IConfiguration _config;

    public AuthController(IConfiguration config)
    {
        _config = config;
    }

    [HttpPost, Route("Login")]
    public IActionResult Login(LoginRequest request)
    {
        var jwtHelper = new JwtHelper(_config);
        var token = jwtHelper.GenerateSecurityToken("fake@email.com");  
        return Ok(token);
    }
}