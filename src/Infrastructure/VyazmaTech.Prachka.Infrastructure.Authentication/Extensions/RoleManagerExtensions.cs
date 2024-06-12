using Microsoft.AspNetCore.Identity;
using VyazmaTech.Prachka.Infrastructure.Authentication.Models;

namespace VyazmaTech.Prachka.Infrastructure.Authentication.Extensions;

internal static class RoleManagerExtensions
{
    public static async Task CreateIfNotExists(
        this RoleManager<VyazmaTechIdentityRole> manager,
        string roleName,
        CancellationToken token)
    {
        if (token.IsCancellationRequested)
        {
            return;
        }

        bool existingRole = await manager.RoleExistsAsync(roleName);

        if (existingRole is false)
        {
            await manager.CreateAsync(new VyazmaTechIdentityRole(roleName));
        }
    }
}