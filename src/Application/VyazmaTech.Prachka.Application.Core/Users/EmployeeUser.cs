using VyazmaTech.Prachka.Application.Abstractions.Identity;

namespace VyazmaTech.Prachka.Application.Core.Users;

internal class EmployeeUser : ICurrentUser
{
    public EmployeeUser(Guid id)
    {
        Id = id;
    }

    public Guid? Id { get; }

    public bool CanChangeUserRole(string currentRoleName, string newRoleName)
    {
        return false;
    }
}