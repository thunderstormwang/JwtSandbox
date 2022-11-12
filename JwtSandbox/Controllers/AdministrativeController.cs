using JwtSandbox.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtSandbox.Controllers;

[Authorize(AuthenticationSchemes = "MyAuth")]
[ApiController]
[Route("api/[controller]")]
public class AdministrativeController : Controller
{
    [Authorize(Roles = "Administrator")]
    [HttpPost, Route("add_account")]
    public IActionResult AddAccount([FromBody] Course course)
    {
        return Ok($"{nameof(AddAccount)} OK");
    }
    
    [Authorize(Roles = "Administrator")]
    [HttpPost, Route("unfrozen_account")]
    public IActionResult UnfrozenAccount([FromBody] Course course)
    {
        return Ok($"{nameof(UnfrozenAccount)} OK");
    }
    
    [Authorize(Roles = "Administrator,Teacher,Student")]
    [HttpPost, Route("dayoff")]
    public IActionResult DayOff([FromBody] Course course)
    {
        return Ok($"{nameof(DayOff)} OK");
    }
}