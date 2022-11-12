using JwtSandbox.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtSandbox.Controllers;

[Authorize(AuthenticationSchemes = "MyAuth")]
[ApiController]
[Route("api/[controller]")]
public class PermissionController : Controller
{
    [HttpGet, Route("authorize")]
    [FetchUserInfo]
    public IActionResult Authorize()
    {
        return Ok("authorize");
    }
}