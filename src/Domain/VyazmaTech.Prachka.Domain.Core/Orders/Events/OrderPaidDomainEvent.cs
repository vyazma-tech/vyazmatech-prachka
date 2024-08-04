using VyazmaTech.Prachka.Domain.Kernel;

namespace VyazmaTech.Prachka.Domain.Core.Orders.Events;

/// <summary>
/// Order is paid. Should be deleted from future queues.
/// </summary>
public sealed class OrderPaidDomainEvent : IDomainEvent
{
    public OrderPaidDomainEvent(Guid id, double price, string fullname)
    {
        Id = id;
        Price = price;
        Fullname = fullname;
    }

    public Guid Id { get; private set; }

    public double Price { get; private set; }

    public string Fullname { get; private set; }

    public static OrderPaidDomainEvent From(Order order)
    {
        return new OrderPaidDomainEvent(order.Id, order.Price, order.User.Fullname);
    }
}