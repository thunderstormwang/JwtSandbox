using JwtSandbox.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtSandbox.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CourseController : Controller
{
    [Authorize(Roles = "Administrator,Teacher,Student")]
    [HttpPost, Route("add")]
    public IActionResult Add([FromBody] Course course)
    {
        var userInfo = HttpContext.Items["user_info"] as UserInfo;
        var courseService = new CourseService();
        courseService.Add(userInfo, course);
        
        return Ok();
    }
}

public class Course
{
}

public class CourseService
{
    public void Add(Course course)
    {
        throw new NotImplementedException();
    }
}