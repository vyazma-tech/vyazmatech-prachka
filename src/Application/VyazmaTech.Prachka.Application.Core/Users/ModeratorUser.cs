using VyazmaTech.Prachka.Application.Abstractions.Identity;
using VyazmaTech.Prachka.Application.Abstractions.Identity.Models;

namespace VyazmaTech.Prachka.Application.Core.Users;

internal sealed class ModeratorUser : ICurrentUser
{
    public ModeratorUser(Guid id)
    {
        Id = id;
    }

    public Guid? Id { get; }

    public bool CanChangeUserRole(string currentRoleName, string newRoleName)
    {
        if (string.Equals(currentRoleName, VyazmaTechRoleNames.AdminRoleName, StringComparison.OrdinalIgnoreCase))
            return false;

        if (string.Equals(newRoleName, VyazmaTechRoleNames.AdminRoleName, StringComparison.OrdinalIgnoreCase))
            return false;

        return true;
    }
}