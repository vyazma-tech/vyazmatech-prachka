using Domain.Common.Errors;
using Domain.Common.Exceptions;
using Domain.Common.Result;
using Domain.Core.Order;
using Domain.Core.User;
using Infrastructure.DataAccess.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccess.Repositories;

internal class OrderRepository : GenericRepository, IOrderRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OrderRepository"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    public OrderRepository(DatabaseContext dbContext) : base(dbContext)
    {
    }

    public async Task<Result<OrderEntity>> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        OrderEntity? entity = await DbContext.Orders.
            FirstOrDefaultAsync(u => u.Id == id, cancellationToken: cancellationToken);
        
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
        List<OrderEntity> entity = await DbContext.Orders.
            Where(u => u.CreationDate == creationDateUtc)
            .ToListAsync(cancellationToken: cancellationToken);

        return entity;
    }

    public async Task<Result<IReadOnlyCollection<OrderEntity>>> FindByUserAsync(UserEntity user,
        CancellationToken cancellationToken)
    {
        List<OrderEntity> entity = await DbContext.Orders.
            Where(u => u.User == user)
            .ToListAsync(cancellationToken: cancellationToken);

        return entity;
    }

    public async Task InsertRangeAsync(IReadOnlyCollection<OrderEntity> orders, CancellationToken cancellationToken)
    {
        await DbContext.Orders.AddRangeAsync(orders, cancellationToken);
    }

    public void Update(OrderEntity order)
    {
        DbContext.Orders.Update(order);
    }

    public async Task<long> CountAsync(CancellationToken cancellationToken)
    {
        return await DbContext.Orders.CountAsync(cancellationToken: cancellationToken);
    }
}