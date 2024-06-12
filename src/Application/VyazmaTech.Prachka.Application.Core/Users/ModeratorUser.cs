using VyazmaTech.Prachka.Application.Abstractions.Identity;

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
        return true;
    }
}