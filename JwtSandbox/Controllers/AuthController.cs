using JwtSandbox.Models;
using Microsoft.AspNetCore.Mvc;

namespace JwtSandbox.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : Controller
{
    [HttpPost, Route("Login")]
    public IActionResult Login(LoginRequest request)
    {
        return Ok("hello world");
    }
}