using Application.DataAccess.Contracts.Querying.Order;
using Domain.Core.Order;

namespace Application.DataAccess.Contracts.Repositories;

public interface IOrderRepository
{
    IAsyncEnumerable<OrderEntity> QueryAsync(
        OrderQuery query,
        CancellationToken cancellationToken);

    void InsertRange(IReadOnlyCollection<OrderEntity> orders);

    void Update(OrderEntity order);

    Task<long> CountAsync(OrderQuery query, CancellationToken cancellationToken);
}