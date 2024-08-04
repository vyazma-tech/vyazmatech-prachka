using VyazmaTech.Prachka.Domain.Kernel;

namespace VyazmaTech.Prachka.Domain.Core.Orders.Events;

/// <summary>
/// Order is ready. If it has subscribers, they should
/// be notified. Should be removed from a current queue.
/// </summary>
public sealed class OrderReadyDomainEvent : IDomainEvent
{
    public OrderReadyDomainEvent(Guid id, string fullname)
    {
        Id = id;
        Fullname = fullname;
    }

    public Guid Id { get; }

    public string Fullname { get; }

    public static OrderReadyDomainEvent From(Order order)
    {
        return new OrderReadyDomainEvent(order.Id, order.User.Fullname);
    }
}