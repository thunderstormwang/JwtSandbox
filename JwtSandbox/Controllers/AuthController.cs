using JwtSandbox.Models;
using JwtSandbox.Services;
using Microsoft.AspNetCore.Mvc;

namespace JwtSandbox.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : Controller
{
    private IConfiguration _config;

    public AuthController(IConfiguration config)
    {
        _config = config;
    }

    [HttpPost, Route("Login")]
    public IActionResult Login(LoginRequest request)
    {
        var jwt = new JwtService(_config);
        var token = jwt.GenerateSecurityToken("fake@email.com");  
        return Ok(token);
    }
}