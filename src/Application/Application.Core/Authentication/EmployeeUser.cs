using Application.Core.Abstractions;

namespace Application.Core.Authentication;

internal class EmployeeUser : ICurrentUser
{
    public EmployeeUser(Guid id)
    {
        Id = id;
    }

    public Guid? Id { get; }

    public bool CanChangeUserRole(string currentRoleName, string newRoleName)
        => false;
}