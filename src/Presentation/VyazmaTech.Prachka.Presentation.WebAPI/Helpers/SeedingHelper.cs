using Mediator;
using VyazmaTech.Prachka.Application.Abstractions.Identity;
using VyazmaTech.Prachka.Application.Abstractions.Identity.Models;
using VyazmaTech.Prachka.Application.Contracts.Identity.Commands;
using VyazmaTech.Prachka.Presentation.WebAPI.Models;

namespace VyazmaTech.Prachka.Presentation.WebAPI.Helpers;

internal static class SeedingHelper
{
    public static async Task SeedRoles(this IServiceScope scope)
    {
        IAuthenticationService service = scope.ServiceProvider.GetRequiredService<IAuthenticationService>();

        await service.CreateRoleIfNotExistsAsync(VyazmaTechRoleNames.AdminRoleName, CancellationToken.None);
        await service.CreateRoleIfNotExistsAsync(VyazmaTechRoleNames.ModeratorRoleName, CancellationToken.None);
        await service.CreateRoleIfNotExistsAsync(VyazmaTechRoleNames.EmployeeRoleName, CancellationToken.None);
        await service.CreateRoleIfNotExistsAsync(VyazmaTechRoleNames.UserRoleName, CancellationToken.None);
    }

    public static async Task SeedDefaultAdmins(this IServiceScope scope, IConfiguration configuration)
    {
        ISender sender = scope.ServiceProvider.GetRequiredService<ISender>();
        ILogger<Program> logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

        AdminModel[]? admins = configuration.GetSection(AdminModel.SectionKey).Get<AdminModel[]>();

        foreach (AdminModel admin in admins ?? Array.Empty<AdminModel>())
        {
            var command = new RegisterUser.Command(
                Guid.NewGuid(),
                new IdentityUserCredentials(
                    admin.Username,
                    admin.Fullname,
                    admin.TelegramId,
                    admin.TelegramImageUrl),
                VyazmaTechRoleNames.AdminRoleName);

            try
            {
                await sender.Send(command);
            }
            catch (Exception e)
            {
                logger.LogWarning(
                    e,
                    "Error occured during seeding admin TelegramUsername = {TelegramUsername}",
                    admin.Username);
            }
        }
    }
}