using VyazmaTech.Prachka.Domain.Core.User;

namespace VyazmaTech.Prachka.Domain.Core.Order;

public sealed class OrderInfo
{
    public OrderInfo(Guid id, UserInfo user, Guid queue, OrderStatus status)
    {
        Id = id;
        User = user;
        Queue = queue;
        Status = status;
    }

    public Guid Id { get; }

    public UserInfo User { get; }

    public Guid Queue { get; }

    public OrderStatus Status { get; }
}