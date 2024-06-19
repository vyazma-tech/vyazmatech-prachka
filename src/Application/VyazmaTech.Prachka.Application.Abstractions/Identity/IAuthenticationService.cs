using System.Security.Claims;
using VyazmaTech.Prachka.Application.Abstractions.Identity.Models;

namespace VyazmaTech.Prachka.Application.Abstractions.Identity;

public interface IAuthenticationService
{
    Task<IdentityTokenModel> GetUserTokensAsync(string telegramUsername, CancellationToken token);

    Task<IdentityTokenModel> RefreshToken(string accessToken, string refreshToken);

    Task RevokeToken(string telegramUsername, CancellationToken token);

    ClaimsPrincipal? DecodePrincipal(string token);

    ClaimsPrincipal DecodePrincipalFromExpiredToken(string token);

    Task CreateRoleIfNotExistsAsync(string roleName, CancellationToken token);

    Task<IdentityUserModel> CreateUserAsync(
        Guid userId,
        IdentityUserCredentials credentials,
        string roleName,
        CancellationToken token);

    Task<IdentityUserModel> GetUserByIdAsync(Guid userId, CancellationToken token);

    Task<IdentityUserModel> GetUserByTelegramUsernameAsync(string telegramUsername, CancellationToken token);

    Task UpdateUserRoleAsync(Guid userId, string newRoleName, CancellationToken token);

    Task UpdateUserRoleAsync(string telegramUsername, string newRoleName, CancellationToken token);

    Task<string> GetUserRoleAsync(Guid userId, CancellationToken token);

    Task<string> GetUserRoleAsync(string telegramUsername, CancellationToken token);
}