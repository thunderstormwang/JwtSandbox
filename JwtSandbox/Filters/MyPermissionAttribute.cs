using System.Text.Json;
using JwtSandbox.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace JwtSandbox.Filters;

public class MyPermissionAttribute : AuthorizeAttribute, IAuthorizationFilter
{
     private readonly MyFunction _functionEnum;
     private readonly MyAction[] _actionEnums;
     
     public MyPermissionAttribute(MyFunction functionEnum, MyAction[] actionEnums)
     {
          _functionEnum = functionEnum;
          _actionEnums = actionEnums;
     }
     
     /// <inheritdoc />
     public void OnAuthorization(AuthorizationFilterContext context)
     {
          var userFunctionDict = JsonSerializer.Deserialize<Dictionary<MyFunction, MyAction[]>>(context.HttpContext.User.Claims.Where(c => c.Type == "function").FirstOrDefault()?.Value);
          
          if (userFunctionDict == null)
          {
               context.Result = new MyMethodNotAllowedResult();
               return;
          }

          if (!userFunctionDict.ContainsKey(_functionEnum))
          {
               context.Result = new MyMethodNotAllowedResult();
               return;
          }
          
          var userActions = userFunctionDict[_functionEnum];
          if (!userActions.Any(u => _actionEnums.Contains(u)))
          {
               context.Result = new MyMethodNotAllowedResult();
               return;
          }
          
          // 符合權限
          return;
     }
}

/// <summary>
/// A <see cref="StatusCodeResult"/> that when
/// executed will produce a Bad Request (405) response.
/// </summary>
[DefaultStatusCode(DefaultStatusCode)]
public class MyMethodNotAllowedResult : StatusCodeResult
{
     private const int DefaultStatusCode = StatusCodes.Status405MethodNotAllowed;

     /// <summary>
     /// Creates a new <see cref="MyMethodNotAllowedResult"/> instance.
     /// </summary>
     public MyMethodNotAllowedResult()
          : base(DefaultStatusCode)
     {
     }
}