using System.Security.Claims;
using Microsoft.AspNetCore.DataProtection;

namespace RawCookieAuthentication;

internal class AuthService(IDataProtectionProvider dataProtectionProvider, IHttpContextAccessor accessor)
{
    private const string AuthCookiePurpose = "auth-cookie";

    private IDataProtector? _protector = null;
    private IDataProtector Protector => _protector ??= dataProtectionProvider.CreateProtector(AuthCookiePurpose);

    private HttpContext HttpContext => accessor.HttpContext!;

    public void SignIn()
    {
        var expiresAt = DateTime.Now.AddDays(1);
        var maxAge = (expiresAt - DateTime.Now).TotalSeconds;
        HttpContext.Response.Headers["set-cookie"] = $"auth={Protector.Protect("user:felipe")}; expires={expiresAt:R}; max-age:{maxAge}";
    }

    public void SignOut()
    {
        HttpContext.Response.Headers["set-cookie"] = "auth=; expires=Thu, 01 Jan 1970 00:00:00 GMT; max-age: 0";
        HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity());
    }

    public void SetPrincipal()
    {
        var authCookie = HttpContext.Request.Headers.Cookie.FirstOrDefault(x => x != null && x.StartsWith("auth="));

        if (string.IsNullOrWhiteSpace(authCookie))
            return;

        var protectedPayload = authCookie.Split('=').Last();
        var payload = Protector.Unprotect(protectedPayload);
        var parts = payload.Split(':');
        var key = parts[0];
        var value = parts[1];

        var claims = new[] { new Claim(key, value) };
        var identity = new ClaimsIdentity(claims, "custom");
        HttpContext.User = new ClaimsPrincipal(identity);
    }

    public bool IsAuthenticated() => HttpContext.User.Identity is { IsAuthenticated: true };

    public ClaimsPrincipal GetPrincipal() => HttpContext.User;
}