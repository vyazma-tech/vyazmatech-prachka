namespace Application.Core.Abstractions;

public interface ICurrentUser
{
    Guid? Id { get; }

    bool CanChangeUserRole(string currentRoleName, string newRoleName);
}