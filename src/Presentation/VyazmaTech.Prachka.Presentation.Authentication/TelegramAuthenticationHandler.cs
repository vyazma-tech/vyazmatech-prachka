using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using VyazmaTech.Prachka.Application.Abstractions.Identity;
using IAuthenticationService = VyazmaTech.Prachka.Application.Abstractions.Identity.IAuthenticationService;

namespace VyazmaTech.Prachka.Presentation.Authentication;

public class TelegramAuthenticationHandler : AuthenticationHandler<TelegramAuthenticationOptions>
{
    public TelegramAuthenticationHandler(
        IOptionsMonitor<TelegramAuthenticationOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder) : base(options, logger, encoder) { }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        IServiceProvider services = Request.HttpContext.RequestServices;

        IAuthenticationService authService = services.GetRequiredService<IAuthenticationService>();
        ICurrentUserManager userManager = services.GetRequiredService<ICurrentUserManager>();
        IBanUserService banService = services.GetRequiredService<IBanUserService>();

        string? authorizationHeader = Request.Headers.Authorization.ToString();

        if (string.IsNullOrEmpty(authorizationHeader))
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        string token = authorizationHeader[(Scheme.Name.Length + 1)..];

        if (string.IsNullOrEmpty(token))
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        ClaimsPrincipal? principal = authService.DecodePrincipal(token);
        if (principal is null)
        {
            return Task.FromResult(AuthenticateResult.Fail("Invalid token"));
        }

        string username = principal.Claims.First(x => x.Type == ClaimTypes.Name).Value;
        if (banService.IsBanned(username))
        {
            return Task.FromResult(AuthenticateResult.Fail("You are banned"));
        }

        JwtSecurityToken? securityToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
        principal = new ClaimsPrincipal(new ClaimsIdentity(securityToken.Claims, "Jwt"));

        userManager.Authenticate(principal);
        return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(principal, Scheme.Name)));
    }
}