using Domain.Common.Result;
using Domain.Core.Order;
using Infrastructure.DataAccess.Models;

namespace Infrastructure.DataAccess.Contracts;

public interface IOrderRepository
{
    Task<Result<OrderEntity>> FindByAsync(
        Specification<OrderModel> specification,
        CancellationToken cancellationToken);

    IAsyncEnumerable<OrderEntity> FindAllByAsync(
        Specification<OrderModel> specification,
        CancellationToken cancellationToken);

    void InsertRange(IReadOnlyCollection<OrderEntity> orders);

    void Update(OrderEntity order);

    Task<long> CountAsync(Specification<OrderModel> specification, CancellationToken cancellationToken);
}