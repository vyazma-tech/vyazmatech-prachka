using Domain.Common.Abstractions;

namespace Domain.Core.Subscriber.Events;

public sealed class EndOfDayDomainEvent : IDomainEvent
{
    public EndOfDayDomainEvent(SubscriberEntity subscriber)
    {
        Subscriber = subscriber;
    }

    public SubscriberEntity Subscriber { get; set; }
}