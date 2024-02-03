using Domain.Common.Result;
using Infrastructure.Authentication.Errors;
using Infrastructure.Authentication.Models;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Authentication.Extensions;

public static class UserManagerExtensions
{
    internal static async Task<Result<VyazmaTechIdentityUser>> GetByTelegramUsernameAsync(
        this UserManager<VyazmaTechIdentityUser> manager,
        string telegramUsername,
        CancellationToken token)
    {
        VyazmaTechIdentityUser? user = await manager.FindByNameAsync(telegramUsername);

        if (user is null)
        {
            return new Result<VyazmaTechIdentityUser>(
                AuthenticationErrors.IdentityUser.NotFoundFor($"TelegramUsername = {telegramUsername}"));
        }

        return user;
    }

    internal static async Task<Result<VyazmaTechIdentityUser>> GetByIdAsync(
        this UserManager<VyazmaTechIdentityUser> manager,
        Guid id,
        CancellationToken token)
    {
        VyazmaTechIdentityUser? user = await manager.FindByIdAsync(id.ToString());

        if (user is null)
        {
            return new Result<VyazmaTechIdentityUser>(
                AuthenticationErrors.IdentityUser.NotFoundFor($"UserId = {id}"));
        }

        return user;
    }
}