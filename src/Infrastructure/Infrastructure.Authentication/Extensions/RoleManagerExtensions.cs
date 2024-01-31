using Infrastructure.Authentication.Models;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Authentication.Extensions;

internal static class RoleManagerExtensions
{
    public static async Task CreateIfNotExists(
        this RoleManager<VyazmaTechIdentityRole> manager,
        string roleName,
        CancellationToken token)
    {
        if (token.IsCancellationRequested)
            return;

        bool existingRole = await manager.RoleExistsAsync(roleName);

        if (existingRole is false)
            await manager.CreateAsync(new VyazmaTechIdentityRole(roleName));
    }
}