using Domain.Core.Subscription;
using Infrastructure.DataAccess.Contexts;
using Infrastructure.DataAccess.Contracts;
using Infrastructure.DataAccess.Models;

namespace Infrastructure.DataAccess.Repositories;

internal class OrderSubscriptionRepositoryBase : RepositoryBase<OrderSubscriptionEntity, OrderSubscriptionModel>, IOrderSubscriptionRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OrderSubscriptionRepositoryBase" /> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public OrderSubscriptionRepositoryBase(DatabaseContext context)
        : base(context)
    {
    }
}