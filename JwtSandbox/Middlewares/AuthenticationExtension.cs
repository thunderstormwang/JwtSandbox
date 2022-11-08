using System.Security.Claims;
using System.Text;
using JwtSandbox.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace JwtSandbox.Middlewares;

public static class AuthenticationExtension
{
    public static IServiceCollection AddTokenAuthentication(this IServiceCollection services, IConfiguration config)
    {
        var authSetting = config.GetSection("AuthSetting").Get<AuthSetting>();
        var key = Encoding.ASCII.GetBytes(authSetting.Secret);
        
        services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    // 透過這項宣告，就可以從 "sub" 取值並設定給 User.Identity.Name
                    NameClaimType = ClaimTypes.NameIdentifier,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = authSetting.Issuer,
                    ValidateAudience = true,
                    ValidAudience = authSetting.Audience,
                    ValidateLifetime = true
                };
            });

        return services;
    }
}