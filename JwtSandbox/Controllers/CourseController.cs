﻿using JwtSandbox.Filters;
using JwtSandbox.Models;
using JwtSandbox.Models.Enums;
using JwtSandbox.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtSandbox.Controllers;

[Authorize(AuthenticationSchemes = "MyAuth")]
[ApiController]
[Route("api/[controller]")]
public class CourseController : Controller
{
    [Authorize(Roles = "Administrator,Teacher")]
    [HttpPost, Route("set_course")]
    [FetchUserInfo]
    public IActionResult SetCourse([FromBody] Course course)
    {
        var userInfo = HttpContext.Items["user_info"] as UserInfo;
        var courseService = new CourseService();
        courseService.Add(userInfo, course);
        
        return Ok($"{nameof(SetCourse)} OK");
    }
    
    [Authorize(Roles = "Administrator,Teacher")]
    [HttpPost, Route("cancel_course")]
    public IActionResult CancelCourse([FromBody] Course course)
    {
        return Ok($"{nameof(CancelCourse)} OK");
    }
    
    [Authorize(Roles = "Administrator,Student")]
    [HttpPost, Route("major_course")]
    public IActionResult MajorCourse([FromBody] Course course)
    {
        return Ok($"{nameof(MajorCourse)} OK");
    }
    
    [Authorize(Roles = "Administrator,Student")]
    [HttpPost, Route("withdraw_course")]
    public IActionResult WithdrawCourse([FromBody] Course course)
    {
        return Ok($"{nameof(WithdrawCourse)} OK");
    }
}