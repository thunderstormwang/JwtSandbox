using System.Security.Claims;
using JwtSandbox.Models;
using Microsoft.AspNetCore.Mvc.Filters;

namespace JwtSandbox.Filters;

public class FetchUserInfoAttribute : ActionFilterAttribute
{
    /// <inheritdoc />
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.HttpContext.User.Identity.IsAuthenticated)
        {
            return;
        }
        
        var userInfo = new UserInfo()
        {
            UserId = context.HttpContext.User.Identity?.Name,
            DisplayName = context.HttpContext.User.Claims.Where(c => c.Type == "display_name")?.FirstOrDefault()?.Value,
            Email = context.HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Email)?.FirstOrDefault()?.Value,
            Roles = context.HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Role)?.Select(c => c.Value).ToList(),
        };
        context.HttpContext.Items["user_info"] = userInfo;
    }
}