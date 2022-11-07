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
    
    [Authorize(Roles = "Administrator")]
    [HttpGet, Route("administrator")]
    public IActionResult Administrator()
    {
        return Ok("authorize administrator");
    }
    
    [Authorize(Roles = "Administrator,Teacher,Student")]
    [HttpGet, Route("user")]
    public IActionResult User()
    {
        return Ok("authorize user");
    }
    
    [Authorize(Roles = "Administrator,Teacher")]
    [HttpGet, Route("teacher")]
    public IActionResult Teacher()
    {
        return Ok("authorize teacher");
    }
}