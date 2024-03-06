namespace VyazmaTech.Prachka.Application.Abstractions.Identity;

public interface ICurrentUser
{
    Guid? Id { get; }

    bool CanChangeUserRole(string currentRoleName, string newRoleName);
}