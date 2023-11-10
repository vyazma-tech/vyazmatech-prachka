using Domain.Common.Result;
using Domain.Core.User;

namespace Domain.Core.Order;

public interface IOrderRepository
{
    Task<Result<OrderEntity>> FindByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<Result<IReadOnlyCollection<OrderEntity>>> FindByCreationDateAsync(DateTime creationDateUtc, CancellationToken cancellationToken);

    Task<Result<IReadOnlyCollection<OrderEntity>>> FindByUserAsync(UserEntity user, CancellationToken cancellationToken);

    Task InsertRangeAsync(IReadOnlyCollection<OrderEntity> orders, CancellationToken cancellationToken);

    void Update(OrderEntity order);

    Task<long> CountAsync(CancellationToken cancellationToken);
}