using System.Security.Claims;
using VyazmaTech.Prachka.Application.Abstractions.Identity.Models;
using VyazmaTech.Prachka.Domain.Common.Result;

namespace VyazmaTech.Prachka.Application.Abstractions.Identity;

public interface IAuthenticationService
{
    Task<Result<IdentityTokenModel>> GetUserTokensAsync(string telegramUsername, CancellationToken token);

    Task<Result<IdentityTokenModel>> RefreshToken(string accessToken, string refreshToken);

    Task<Result> RevokeToken(string telegramUsername, CancellationToken token);

    ClaimsPrincipal? DecodePrincipal(string token);

    ClaimsPrincipal DecodePrincipalFromExpiredToken(string token);

    Task CreateRoleIfNotExistsAsync(string roleName, CancellationToken token);

    Task<Result<IdentityUserModel>> CreateUserAsync(
        Guid userId,
        IdentityUserCredentials credentials,
        string roleName,
        CancellationToken token);

    Task<Result<IdentityUserModel>> GetUserByIdAsync(Guid userId, CancellationToken token);

    Task<Result<IdentityUserModel>> GetUserByTelegramUsernameAsync(string telegramUsername, CancellationToken token);

    Task<Result> UpdateUserRoleAsync(Guid userId, string newRoleName, CancellationToken token);

    Task<Result> UpdateUserRoleAsync(string telegramUsername, string newRoleName, CancellationToken token);

    Task<Result<string>> GetUserRoleAsync(Guid userId, CancellationToken token);

    Task<Result<string>> GetUserRoleAsync(string telegramUsername, CancellationToken token);
}