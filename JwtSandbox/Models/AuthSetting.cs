namespace JwtSandbox.Models;

public class AuthSetting
{
    public string Secret { get; set; }

    public int ExpirationInMinutes { get; set; }

    public string Issuer { get; set; }

    public string Audience { get; set; }

    public string AuthorizationHeaderName { get; set; }

    public string BackdoorHeaderName { get; set; }

    public string BackdoorKeyword { get; set; }
}