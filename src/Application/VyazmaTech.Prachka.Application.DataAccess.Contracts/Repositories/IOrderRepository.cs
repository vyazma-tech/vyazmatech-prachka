using VyazmaTech.Prachka.Domain.Core.Orders;

namespace VyazmaTech.Prachka.Application.DataAccess.Contracts.Repositories;

public interface IOrderRepository
{
    Task<Order> GetByIdAsync(Guid id, CancellationToken token);

    IAsyncEnumerable<Order> QueryByUserAsync(Guid id, CancellationToken token);

    void InsertRange(IReadOnlyCollection<Order> orders);

    void RemoveRange(IReadOnlyCollection<Order> orders);
}