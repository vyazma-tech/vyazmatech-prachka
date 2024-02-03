using Application.Core.Abstractions;

namespace Application.Core.Authentication;

internal class AnonymousUser : ICurrentUser
{
    public Guid? Id => null;

    public bool CanChangeUserRole(string currentRoleName, string newRoleName)
        => false;
}