using Domain.Core.Order;
using Infrastructure.DataAccess.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccess.Repositories;

internal class OrderRepository : GenericRepository<OrderEntity>, IOrderRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OrderRepository"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public OrderRepository(DatabaseContext context)
        : base(context)
    {
    }

    public async Task<long> CountAsync(CancellationToken cancellationToken)
    {
        return await DbSet.CountAsync(cancellationToken: cancellationToken);
    }
}