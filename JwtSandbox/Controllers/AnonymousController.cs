using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtSandbox.Controllers;

[AllowAnonymous]
[ApiController]
[Route("api/[controller]")]
public class AnonymousController : Controller
{
    [HttpGet, Route("anonymous")]
    public IActionResult Anonymous()
    {
        return Ok("anonymous");
    }
}