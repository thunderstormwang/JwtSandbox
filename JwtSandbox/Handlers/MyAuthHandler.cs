using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace JwtSandbox.Middlewares;

public class MyAuthHandler : AuthenticationHandler<MyAuthenticationSchemeOptions>
{
    /// <inheritdoc />
    public MyAuthHandler(IOptionsMonitor<MyAuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
    {
    }

    /// <inheritdoc />
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // 取得自定義的欄位
        var hello = base.Options.Hello;
        return AuthenticateResult.Success();
    }
}