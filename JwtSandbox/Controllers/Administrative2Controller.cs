using JwtSandbox.Filters;
using JwtSandbox.Models;
using JwtSandbox.Models.Enums;
using JwtSandbox.Services;
using Microsoft.AspNetCore.Mvc;

namespace JwtSandbox.Controllers;

[ApiController]
[Route("api/[controller]")]
public class Administrative2Controller : Controller
{
    [MyPermission(MyFunction.AddAccount, new MyAction[] { MyAction.All })]
    [HttpPost, Route("add_account")]
    public IActionResult AddAccount([FromBody] Course course)
    {
        return Ok($"{nameof(AddAccount)} OK");
    }
    
    [MyPermission(MyFunction.UnFrozenAccount, new MyAction[] { MyAction.All })]
    [HttpPost, Route("unfrozen_account")]
    public IActionResult UnfrozenAccount([FromBody] Course course)
    {
        return Ok($"{nameof(UnfrozenAccount)} OK");
    }
    
    [MyPermission(MyFunction.DayOff, new MyAction[] { MyAction.All })]
    [HttpPost, Route("dayoff")]
    public IActionResult DayOff([FromBody] Course course)
    {
        return Ok($"{nameof(DayOff)} OK");
    }
}