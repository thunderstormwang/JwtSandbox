using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace JwtSandbox.Services;

public class JwtService
{
    private string mysecret;
    private string myexpDate;

    public JwtService(IConfiguration config)
    {
        mysecret = config.GetSection("JwtConfig").GetSection("secret").Value;
        myexpDate = config.GetSection("JwtConfig").GetSection("expirationInMinutes").Value;
    }

    public string GenerateSecurityToken(string email)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(mysecret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Iss, "localhost"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Email, email)
            }),
            Audience = "localhost",
            Expires = DateTime.UtcNow.AddMinutes(double.Parse(myexpDate)),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}