using Domain.Common.Result;
using Domain.Core.Order;
using Domain.Core.User;

namespace Application.DataAccess.Contracts.Repositories;

public interface IOrderRepository
{
    Task<Result<OrderEntity>> FindByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<Result<IReadOnlyCollection<OrderEntity>>> FindByCreationDateAsync(DateTime creationDateUtc, CancellationToken cancellationToken);

    Task<Result<IReadOnlyCollection<OrderEntity>>> FindByUserAsync(UserEntity user, CancellationToken cancellationToken);
    
    Task InsertRangeAsync(IReadOnlyCollection<OrderEntity> orders, CancellationToken cancellationToken);

    Task UpdateAsync(OrderEntity order, CancellationToken cancellationToken);

    Task<long> CountAsync(CancellationToken cancellationToken);
}