using Domain.Common.Abstractions;

namespace Domain.Core.Subscriber.Events;

/// <summary>
/// Day is ended. Subscription should be reset.
/// </summary>
public sealed class EndOfDayDomainEvent : IDomainEvent
{
    public EndOfDayDomainEvent(SubscriberEntity subscriber)
    {
        Subscriber = subscriber;
    }

    public SubscriberEntity Subscriber { get; set; }
}