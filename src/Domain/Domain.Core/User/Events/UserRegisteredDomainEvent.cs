using Domain.Common.Abstractions;

namespace Domain.Core.User.Events;

/// <summary>
///     User registered. Empty subscription should be assigned to them.
/// </summary>
public sealed class UserRegisteredDomainEvent : IDomainEvent
{
    public UserRegisteredDomainEvent(UserEntity user)
    {
        User = user;
    }

    public UserEntity User { get; }
}