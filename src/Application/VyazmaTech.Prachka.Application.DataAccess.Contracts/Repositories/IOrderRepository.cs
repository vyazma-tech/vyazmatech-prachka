using VyazmaTech.Prachka.Application.Abstractions.Querying.Order;
using VyazmaTech.Prachka.Domain.Core.Order;

namespace VyazmaTech.Prachka.Application.DataAccess.Contracts.Repositories;

public interface IOrderRepository
{
    IAsyncEnumerable<OrderEntity> QueryAsync(
        OrderQuery query,
        CancellationToken cancellationToken);

    void InsertRange(IReadOnlyCollection<OrderEntity> orders);

    void Update(OrderEntity order);

    Task<long> CountAsync(OrderQuery query, CancellationToken cancellationToken);
}