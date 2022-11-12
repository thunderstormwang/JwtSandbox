using JwtSandbox.Filters;
using JwtSandbox.Models;
using JwtSandbox.Models.Enums;
using JwtSandbox.Services;
using Microsoft.AspNetCore.Mvc;

namespace JwtSandbox.Controllers;

[ApiController]
[Route("api/[controller]")]
public class Course2Controller : Controller
{
    [MyPermission(MyFunction.SetCourse, new MyAction[] { MyAction.All })]
    [HttpPost, Route("set_course")]
    [FetchUserInfo2]
    public IActionResult SetCourse([FromBody] Course course)
    {
        var userInfo = HttpContext.Items["user_info"] as UserInfo2;
        var courseService = new CourseService();
        courseService.Add(userInfo, course);
        
        return Ok($"{nameof(SetCourse)} OK");
    }
    
    [MyPermission(MyFunction.CancelCourse, new MyAction[] { MyAction.All })]
    [HttpPost, Route("cancel_course")]
    public IActionResult CancelCourse([FromBody] Course course)
    {
        return Ok($"{nameof(CancelCourse)} OK");
    }
    
    [MyPermission(MyFunction.MajorCourse, new MyAction[] { MyAction.All })]
    [HttpPost, Route("major_course")]
    public IActionResult MajorCourse([FromBody] Course course)
    {
        return Ok($"{nameof(MajorCourse)} OK");
    }
    
    [MyPermission(MyFunction.WithdrawCourse, new MyAction[] { MyAction.All })]
    [HttpPost, Route("withdraw_course")]
    public IActionResult WithdrawCourse([FromBody] Course course)
    {
        return Ok($"{nameof(WithdrawCourse)} OK");
    }
}