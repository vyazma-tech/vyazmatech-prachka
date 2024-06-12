using VyazmaTech.Prachka.Domain.Kernel;

namespace VyazmaTech.Prachka.Domain.Core.Users.Events;

/// <summary>
/// User registered. Empty subscription should be assigned to them.
/// </summary>
public sealed class UserRegisteredDomainEvent : IDomainEvent
{
    public UserRegisteredDomainEvent(User user)
    {
        User = user;
    }

    public User User { get; }
}