using Microsoft.AspNetCore.Identity;
using VyazmaTech.Prachka.Domain.Common.Exceptions;
using VyazmaTech.Prachka.Infrastructure.Authentication.Errors;
using VyazmaTech.Prachka.Infrastructure.Authentication.Models;

namespace VyazmaTech.Prachka.Infrastructure.Authentication.Extensions;

public static class UserManagerExtensions
{
    internal static async Task<VyazmaTechIdentityUser> GetByTelegramUsernameAsync(
        this UserManager<VyazmaTechIdentityUser> manager,
        string telegramUsername,
        CancellationToken token)
    {
        VyazmaTechIdentityUser? user = await manager.FindByNameAsync(telegramUsername);

        if (user is null)
        {
            throw new IdentityException(
                AuthenticationErrors.IdentityUser.NotFoundFor($"TelegramUsername = {telegramUsername}"));
        }

        return user;
    }

    internal static async Task<VyazmaTechIdentityUser> GetByIdAsync(
        this UserManager<VyazmaTechIdentityUser> manager,
        Guid id,
        CancellationToken token)
    {
        VyazmaTechIdentityUser? user = await manager.FindByIdAsync(id.ToString());

        if (user is null)
            throw new IdentityException(AuthenticationErrors.IdentityUser.NotFoundFor($"UserId = {id}"));

        return user;
    }
}