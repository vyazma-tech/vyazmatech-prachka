namespace VyazmaTech.Prachka.Domain.Core.Order;

public sealed class OrderInfo
{
    public OrderInfo(Guid id, Guid user, Guid queue, OrderStatus status)
    {
        Id = id;
        User = user;
        Queue = queue;
        Status = status;
    }

    public Guid Id { get; }

    public Guid User { get; }

    public Guid Queue { get; }

    public OrderStatus Status { get; }
}