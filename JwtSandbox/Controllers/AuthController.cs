using JwtSandbox.Helpers;
using JwtSandbox.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtSandbox.Controllers;

[AllowAnonymous]
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
        var userId = 99;
        var roles = new List<MyRole>() { MyRole.Teacher, MyRole.Student };
        var jwtHelper = new JwtHelper(_config);
        var token = jwtHelper.GenerateSecurityToken(userId, request.Account, "fake@email.com", roles);
        return Ok(token);
    }
}