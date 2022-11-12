using JwtSandbox.Models.Enums;

namespace JwtSandbox.Models;

public class UserInfo
{
    public string UserId { get; set; }
    public string DisplayName { get; set; }
    public string Email { get; set; }
    public List<string> Roles { get; set; }
}

public class UserInfo2
{
    public string UserId { get; set; }
    public string DisplayName { get; set; }
    public string Email { get; set; }
    public List<string> Roles { get; set; }
    public Dictionary<MyFunction, MyAction[]> FunctionDict;
}