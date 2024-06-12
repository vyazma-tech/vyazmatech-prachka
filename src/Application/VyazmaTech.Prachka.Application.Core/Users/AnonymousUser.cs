using VyazmaTech.Prachka.Application.Abstractions.Identity;

namespace VyazmaTech.Prachka.Application.Core.Users;

internal class AnonymousUser : ICurrentUser
{
    public Guid? Id => null;

    public bool CanChangeUserRole(string currentRoleName, string newRoleName)
    {
        return false;
    }
}