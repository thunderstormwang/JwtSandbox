using System.Security.Claims;
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
        // 從 token 取得資料
        var name = HttpContext.User.Identity?.Name;
        var displayName = HttpContext.User.Claims.Where(c => c.Type == "display_name")?.FirstOrDefault()?.Value;
        var email = HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Email)?.FirstOrDefault()?.Value;
        var roles = HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Role)?.Select(c => c.Value).ToList();
        return Ok("authorize");
    }
    
    [Authorize(Roles = "Administrator")]
    [HttpGet, Route("administrator")]
    public IActionResult Administrator()
    {
        return Ok("authorize administrator");
    }

    [Authorize(Roles = "Administrator,Teacher")]
    [HttpGet, Route("teacher")]
    public IActionResult Teacher()
    {
        return Ok("authorize teacher");
    }

    [Authorize(Roles = "Administrator,Teacher,Student")]
    [HttpGet, Route("user")]
    public IActionResult User()
    {
        return Ok("authorize user");
    }
}