using JwtSandbox.Helpers;
using JwtSandbox.Models;
using JwtSandbox.Models.Enums;
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

    [HttpPost, Route("Login2")]
    public IActionResult Login2(LoginRequest request)
    {
        var userId = 99;
        var roles = new List<MyRole>() { MyRole.Teacher };
        var functionDict = new Dictionary<MyFunction, MyAction[]>()
        {
            { MyFunction.DayOff, new MyAction[] { MyAction.All } },
            { MyFunction.SetCourse, new MyAction[] { MyAction.All } },
            { MyFunction.CancelCourse, new MyAction[] { MyAction.All } }
        };

        var jwtHelper = new JwtHelper(_config);
        var token = jwtHelper.GenerateSecurityToken2(userId, request.Account, "fake@email.com", roles, functionDict);
        return Ok(token);
    }
}