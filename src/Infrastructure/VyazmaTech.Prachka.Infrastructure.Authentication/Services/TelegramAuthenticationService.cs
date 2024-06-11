using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using VyazmaTech.Prachka.Application.Abstractions.Identity;
using VyazmaTech.Prachka.Application.Abstractions.Identity.Models;
using VyazmaTech.Prachka.Domain.Common.Result;
using VyazmaTech.Prachka.Infrastructure.Authentication.Configuration;
using VyazmaTech.Prachka.Infrastructure.Authentication.Errors;
using VyazmaTech.Prachka.Infrastructure.Authentication.Extensions;
using VyazmaTech.Prachka.Infrastructure.Authentication.Models;

namespace VyazmaTech.Prachka.Infrastructure.Authentication.Services;

internal sealed class TelegramAuthenticationService : IAuthenticationService
{
    private readonly UserManager<VyazmaTechIdentityUser> _userManager;
    private readonly RoleManager<VyazmaTechIdentityRole> _roleManager;
    private readonly TokenConfiguration _configuration;

    public TelegramAuthenticationService(
        UserManager<VyazmaTechIdentityUser> userManager,
        RoleManager<VyazmaTechIdentityRole> roleManager,
        IOptions<TokenConfiguration> configuration)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration.Value;
    }

    public async Task<Result<IdentityTokenModel>> GetUserTokensAsync(string telegramUsername, CancellationToken token)
    {
        Result<VyazmaTechIdentityUser> searchResult = await _userManager
            .GetByTelegramUsernameAsync(telegramUsername, token);

        if (searchResult.IsFaulted)
            return new Result<IdentityTokenModel>(searchResult.Error);

        VyazmaTechIdentityUser user = searchResult.Value;

        SecurityToken accessToken = await GenerateAccessToken(user);

        if (user.RefreshToken is null || user.RefreshTokenExpiryUtc < DateTime.UtcNow)
            user.RefreshToken = GenerateRefreshToken();

        return new IdentityTokenModel(
            new JwtSecurityTokenHandler().WriteToken(accessToken),
            user.RefreshToken ?? string.Empty);
    }

    public async Task<Result<IdentityTokenModel>> RefreshToken(string accessToken, string refreshToken)
    {
        ClaimsPrincipal principal = DecodePrincipalFromExpiredToken(accessToken);

        Result<VyazmaTechIdentityUser> searchResult = await _userManager
            .GetByTelegramUsernameAsync(
                principal.Identity?.Name ?? string.Empty,
                CancellationToken.None);

        if (searchResult.IsFaulted)
            return new Result<IdentityTokenModel>(searchResult.Error);

        VyazmaTechIdentityUser user = searchResult.Value;

        if (user.RefreshToken != refreshToken || user.RefreshTokenExpiryUtc < DateTime.UtcNow)
            return new Result<IdentityTokenModel>(AuthenticationErrors.IdentityToken.Refresh());

        SecurityToken jwtAccessToken = await GenerateAccessToken(user);

        return new IdentityTokenModel(
            new JwtSecurityTokenHandler().WriteToken(jwtAccessToken),
            refreshToken);
    }

    public async Task<Result> RevokeToken(string telegramUsername, CancellationToken token)
    {
        Result<VyazmaTechIdentityUser> searchResult = await _userManager
            .GetByTelegramUsernameAsync(telegramUsername, token);

        if (searchResult.IsFaulted)
            return Result.Failure();

        VyazmaTechIdentityUser user = searchResult.Value;

        user.RefreshToken = null;

        await _userManager.UpdateAsync(user);

        return Result.Success();
    }

    public ClaimsPrincipal? DecodePrincipal(string token)
    {
        TokenValidationParameters validationParameters = CreateValidationParameters(validateLifetime: true);

        var tokenHandler = new JwtSecurityTokenHandler();

        try
        {
            return tokenHandler.ValidateToken(token, validationParameters, out _);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public ClaimsPrincipal DecodePrincipalFromExpiredToken(string token)
    {
        TokenValidationParameters validationParameters = CreateValidationParameters(validateLifetime: false);

        var tokenHandler = new JwtSecurityTokenHandler();

        return tokenHandler.ValidateToken(token, validationParameters, out _);
    }

    public async Task CreateRoleIfNotExistsAsync(string roleName, CancellationToken token)
    {
        await _roleManager.CreateIfNotExists(roleName, token);
    }

    public async Task<Result<IdentityUserModel>> CreateUserAsync(
        Guid userId,
        IdentityUserCredentials credentials,
        string roleName,
        CancellationToken token)
    {
        var user = VyazmaTechIdentityUser.Create(
            userId,
            credentials,
            GenerateRefreshToken(),
            DateTime.UtcNow.AddMinutes(_configuration.AccessTokenExpiresInMinutes),
            Guid.NewGuid());

        IdentityResult result = await _userManager.CreateAsync(user);

        if (result.Succeeded is false)
            return new Result<IdentityUserModel>(result.ToError());

        result = await _userManager.AddToRoleAsync(user, roleName);

        if (result.Succeeded is false)
            return new Result<IdentityUserModel>(result.ToError());

        return new IdentityUserModel(
            user.Id,
            user.Fullname,
            user.UserName ?? string.Empty,
            user.TelegramImageUrl);
    }

    public async Task<Result<IdentityUserModel>> GetUserByIdAsync(Guid userId, CancellationToken token)
    {
        Result<VyazmaTechIdentityUser> searchResult = await _userManager.GetByIdAsync(userId, token);

        if (searchResult.IsFaulted)
            return new Result<IdentityUserModel>(searchResult.Error);

        VyazmaTechIdentityUser user = searchResult.Value;

        return new IdentityUserModel(
            user.Id,
            user.Fullname,
            user.UserName ?? string.Empty,
            user.TelegramImageUrl);
    }

    public async Task<Result<IdentityUserModel>> GetUserByTelegramUsernameAsync(
        string telegramUsername,
        CancellationToken token)
    {
        Result<VyazmaTechIdentityUser> searchResult =
            await _userManager.GetByTelegramUsernameAsync(telegramUsername, token);

        if (searchResult.IsFaulted)
            return new Result<IdentityUserModel>(searchResult.Error);

        VyazmaTechIdentityUser user = searchResult.Value;

        return new IdentityUserModel(
            user.Id,
            user.Fullname,
            user.UserName ?? string.Empty,
            user.TelegramImageUrl);
    }

    public async Task<Result> UpdateUserRoleAsync(Guid userId, string newRoleName, CancellationToken token)
    {
        if (token.IsCancellationRequested)
            return Result.Failure();

        Result<VyazmaTechIdentityUser> searchResult = await _userManager.GetByIdAsync(userId, token);

        if (searchResult.IsFaulted)
            return Result.Failure();

        VyazmaTechIdentityUser user = searchResult.Value;

        IList<string> roles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, roles);
        await _userManager.AddToRoleAsync(user, newRoleName);

        return Result.Success();
    }

    public async Task<Result> UpdateUserRoleAsync(string telegramUsername, string newRoleName, CancellationToken token)
    {
        if (token.IsCancellationRequested)
            return Result.Failure();

        Result<VyazmaTechIdentityUser> searchResult = await _userManager
            .GetByTelegramUsernameAsync(telegramUsername, token);

        if (searchResult.IsFaulted)
            return Result.Failure();

        VyazmaTechIdentityUser user = searchResult.Value;

        IList<string> roles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, roles);
        await _userManager.AddToRoleAsync(user, newRoleName);

        return Result.Success();
    }

    public async Task<Result<string>> GetUserRoleAsync(Guid userId, CancellationToken token)
    {
        Result<VyazmaTechIdentityUser> searchResult = await _userManager.GetByIdAsync(userId, token);

        if (searchResult.IsFaulted)
            return new Result<string>(searchResult.Error);

        VyazmaTechIdentityUser user = searchResult.Value;

        IList<string> roles = await _userManager.GetRolesAsync(user);

        return roles.Single();
    }

    public async Task<Result<string>> GetUserRoleAsync(string telegramUsername, CancellationToken token)
    {
        Result<VyazmaTechIdentityUser> searchResult = await _userManager
            .GetByTelegramUsernameAsync(telegramUsername, token);

        if (searchResult.IsFaulted)
            return new Result<string>(searchResult.Error);

        VyazmaTechIdentityUser user = searchResult.Value;

        IList<string> roles = await _userManager.GetRolesAsync(user);

        return roles.Single();
    }

    private static string GenerateRefreshToken()
    {
        byte[] number = new byte[64];

        using var generator = RandomNumberGenerator.Create();

        generator.GetBytes(number);

        return Convert.ToBase64String(number);
    }

    private TokenValidationParameters CreateValidationParameters(bool validateLifetime = true)
    {
        return new TokenValidationParameters
        {
            ValidAudience = _configuration.Audience,
            ValidIssuer = _configuration.Issuer,
            IssuerSigningKeys = new[]
            {
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Secret)),
            },
            ValidateLifetime = validateLifetime,
        };
    }

    private async Task<SecurityToken> GenerateAccessToken(VyazmaTechIdentityUser user)
    {
        IList<string> roles = await _userManager.GetRolesAsync(user);

        IEnumerable<Claim> claims = roles
            .Select(role => new Claim(ClaimTypes.Role, role))
            .Append(new Claim(JwtRegisteredClaimNames.Sub, user.UserName ?? string.Empty))
            .Append(new Claim(JwtRegisteredClaimNames.Name, user.Fullname))
            .Append(new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()))
            .Append(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.Secret));

        var jwtToken = new JwtSecurityToken(
            issuer: _configuration.Issuer,
            audience: _configuration.Audience,
            expires: DateTime.UtcNow.AddMinutes(_configuration.AccessTokenExpiresInMinutes),
            claims: claims,
            signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256));

        return jwtToken;
    }
}