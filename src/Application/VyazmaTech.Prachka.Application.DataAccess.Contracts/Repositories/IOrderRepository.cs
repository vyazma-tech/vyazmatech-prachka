using VyazmaTech.Prachka.Application.Abstractions.Querying.Order;
using VyazmaTech.Prachka.Domain.Core.Orders;

namespace VyazmaTech.Prachka.Application.DataAccess.Contracts.Repositories;

public interface IOrderRepository
{
    IAsyncEnumerable<Order> QueryAsync(
        OrderQuery query,
        CancellationToken cancellationToken);

    void InsertRange(IReadOnlyCollection<Order> orders);

    void RemoveRange(IReadOnlyCollection<Order> orders);

    void Update(Order order);

    Task<long> CountAsync(OrderQuery query, CancellationToken cancellationToken);
}