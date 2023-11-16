using Domain.Common.Errors;
using Domain.Common.Exceptions;
using Domain.Common.Result;
using Domain.Core.Order;
using Domain.Core.User;
using Infrastructure.DataAccess.Contexts;
using Infrastructure.DataAccess.Specifications.Order;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccess.Repositories;

internal class OrderRepository : GenericRepository<OrderEntity>, IOrderRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OrderRepository"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public OrderRepository(DatabaseContext context) : base(context)
    {
    }

    public async Task<Result<OrderEntity>> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        OrderEntity? entity = await
            ApplySpecification(new OrderByIdSpecification(id))
                .FirstOrDefaultAsync(cancellationToken);

        if (entity is null)
        {
            var exception = new DomainException(DomainErrors.Order.NotFound);
            return new Result<OrderEntity>(exception);
        }

        return entity;
    }

    public async Task<Result<IReadOnlyCollection<OrderEntity>>> FindByCreationDateAsync(
        DateTime creationDateUtc,
        CancellationToken cancellationToken
    )
    {
        return await
            ApplySpecification(new OrderByDateSpecification(creationDateUtc))
                .ToListAsync(cancellationToken);
    }

    public async Task<Result<IReadOnlyCollection<OrderEntity>>> FindByUserAsync(
        UserEntity user,
        CancellationToken cancellationToken)
    {
        return await
            ApplySpecification(new OrderByUserSpecification(user))
                .ToListAsync(cancellationToken);
    }

    public async Task<long> CountAsync(CancellationToken cancellationToken)
    {
        return await DbSet.CountAsync(cancellationToken: cancellationToken);
    }
}