using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtSandbox.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PermissionController : Controller
{
    [AllowAnonymous]
    [HttpGet, Route("anonymous")]
    public IActionResult Anonymous()
    {
        return Ok("anonymous");
    }
    
    [Authorize]
    [HttpGet, Route("authorize")]
    public IActionResult Authorize()
    {
        return Ok("authorize");
    }
}