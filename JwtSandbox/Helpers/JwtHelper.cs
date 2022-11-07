using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtSandbox.Models;
using Microsoft.IdentityModel.Tokens;

namespace JwtSandbox.Helpers;

public class JwtHelper
{
    private readonly AuthSetting _authSetting;

    public JwtHelper(IConfiguration config)
    {
        _authSetting = config.GetSection("AuthSetting").Get<AuthSetting>();
    }

    public string GenerateSecurityToken(string email)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_authSetting.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Iss, _authSetting.Issuer),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Email, email)
            }),
            Audience = _authSetting.Audience,
            Expires = DateTime.UtcNow.AddMinutes(_authSetting.ExpirationInMinutes),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}