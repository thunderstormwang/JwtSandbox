using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using JwtSandbox.Models;
using JwtSandbox.Models.Enums;
using Microsoft.IdentityModel.Tokens;

namespace JwtSandbox.Helpers;

public class JwtHelper
{
    private readonly AuthSetting _authSetting;

    public JwtHelper(IConfiguration config)
    {
        _authSetting = config.GetSection("AuthSetting").Get<AuthSetting>();
    }
    
    public string GenerateSecurityToken(int userId, string userDisplayName, string email, List<MyRole> roles)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_authSetting.Secret);

        var claims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim("display_name", userDisplayName),
            new Claim(JwtRegisteredClaimNames.Iss, _authSetting.Issuer),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, email)
        };
        claims.AddRange(roles.Select(r => new Claim("role", r.ToString())));
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims.ToArray()),
            Audience = _authSetting.Audience,
            Expires = DateTime.UtcNow.AddMinutes(_authSetting.ExpirationInMinutes),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    public string GenerateSecurityToken2(int userId, string userDisplayName, string email, List<MyRole> roles,
        Dictionary<MyFunction, MyAction[]> functionDict)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_authSetting.Secret);

        var claims = new List<Claim>()
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim("display_name", userDisplayName),
            new Claim(JwtRegisteredClaimNames.Iss, _authSetting.Issuer),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim("role", JsonSerializer.Serialize(roles)),
            new Claim("function", JsonSerializer.Serialize(functionDict)),
        };
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims.ToArray()),
            Audience = _authSetting.Audience,
            Expires = DateTime.UtcNow.AddMinutes(_authSetting.ExpirationInMinutes),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}