using VyazmaTech.Prachka.Domain.Core.Order;

namespace VyazmaTech.Prachka.Domain.Core.Queue;

public sealed class OrderByIdComparer : IEqualityComparer<OrderInfo>
{
    public bool Equals(OrderInfo? x, OrderInfo? y)
    {
        return x?.Id.Equals(y?.Id) ?? false;
    }

    public int GetHashCode(OrderInfo obj)
    {
        return HashCode.Combine(obj.Id, obj.User, obj.Queue, (int)obj.Status);
    }
}