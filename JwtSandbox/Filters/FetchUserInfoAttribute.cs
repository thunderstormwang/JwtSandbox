using System.Security.Claims;
using System.Text.Json;
using JwtSandbox.Models;
using JwtSandbox.Models.Enums;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.JsonWebTokens;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;

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
            DisplayName = context.HttpContext.User.Claims.Where(c => c.Type == "display_name").FirstOrDefault()?.Value,
            // DefaultAuthenticateScheme
            // Email = context.HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Email).FirstOrDefault()?.Value,
            // Roles = context.HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList()
            // 自訂 AuthenticationHandler
            Email = context.HttpContext.User.Claims.Where(c => c.Type == JwtRegisteredClaimNames.Email).FirstOrDefault()?.Value,
            Roles = context.HttpContext.User.Claims.Where(c => c.Type == "role").Select(c => c.Value).ToList()
        };
        context.HttpContext.Items["user_info"] = userInfo;
    }
}

public class FetchUserInfo2Attribute : ActionFilterAttribute
{
    /// <inheritdoc />
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.HttpContext.User.Identity.IsAuthenticated)
        {
            return;
        }
        
        var userInfo = new UserInfo2()
        {
            UserId = context.HttpContext.User.Identity?.Name,
            DisplayName = context.HttpContext.User.Claims.Where(c => c.Type == "display_name").FirstOrDefault()?.Value,
            Email = context.HttpContext.User.Claims.Where(c => c.Type == JwtRegisteredClaimNames.Email).FirstOrDefault()?.Value,
            Roles = context.HttpContext.User.Claims.Where(c => c.Type == "role").Select(c => c.Value).ToList(),
            FunctionDict = JsonSerializer.Deserialize<Dictionary<MyFunction, MyAction[]>>(context.HttpContext.User.Claims.Where(c => c.Type == "function").FirstOrDefault()?.Value)
        };
        context.HttpContext.Items["user_info"] = userInfo;
    }
}