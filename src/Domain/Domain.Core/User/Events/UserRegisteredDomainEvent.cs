using Domain.Common.Abstractions;

namespace Domain.Core.User.Events;

public sealed class UserRegisteredDomainEvent : IDomainEvent
{
    public UserRegisteredDomainEvent(UserEntity user)
    {
        User = user;
    }

    public UserEntity User { get; }
}