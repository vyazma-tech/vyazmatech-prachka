using Domain.Common.Abstractions;

namespace Domain.Core.Subscriber.Events;

public sealed class UserSubscribedDomainEvent : IDomainEvent
{
    public UserSubscribedDomainEvent(SubscriberEntity subscriber)
    {
        Subscriber = subscriber;
    }

    public SubscriberEntity Subscriber { get; set; }
}