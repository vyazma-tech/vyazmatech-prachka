using Application.Core.Abstractions;

namespace Application.Core.Authentication;

internal sealed class ModeratorUser : ICurrentUser
{
    public ModeratorUser(Guid id)
    {
        Id = id;
    }

    public Guid? Id { get; }

    public bool CanChangeUserRole(string currentRoleName, string newRoleName)
        => true;
}