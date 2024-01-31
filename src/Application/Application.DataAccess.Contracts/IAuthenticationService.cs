using System.Security.Claims;
using Application.DataAccess.Contracts.Abstractions;
using Application.Dto.Identity;
using Domain.Common.Result;

namespace Application.DataAccess.Contracts;

public interface IAuthenticationService
{
    Task<Result<IdentityTokenDto>> GetUserTokensAsync(string telegramUsername, CancellationToken token);

    Task<Result<IdentityTokenDto>> RefreshToken(string accessToken, string refreshToken);

    Task<Result> RevokeToken(string telegramUsername, CancellationToken token);

    ClaimsPrincipal? DecodePrincipal(string token);

    ClaimsPrincipal DecodePrincipalFromExpiredToken(string token);

    Task CreateRoleIfNotExistsAsync(string roleName, CancellationToken token);

    Task<Result<IdentityUserDto>> CreateUserAsync(
        Guid userId,
        IdentityUserCredentials credentials,
        string roleName,
        CancellationToken token);

    Task<Result<IdentityUserDto>> GetUserByIdAsync(Guid userId, CancellationToken token);

    Task<Result<IdentityUserDto>> GetUserByTelegramUsernameAsync(string telegramUsername, CancellationToken token);

    Task<Result> UpdateUserRoleAsync(Guid userId, string newRoleName, CancellationToken token);

    Task<Result> UpdateUserRoleAsync(string telegramUsername, string newRoleName, CancellationToken token);

    Task<Result<string>> GetUserRoleAsync(Guid userId, CancellationToken token);

    Task<Result<string>> GetUserRoleAsync(string telegramUsername, CancellationToken token);
}