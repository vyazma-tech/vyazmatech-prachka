using Domain.Core.Subscription;
using Infrastructure.DataAccess.Contexts;

namespace Infrastructure.DataAccess.Repositories;

internal class SubscriptionRepository : GenericRepository<SubscriptionEntity>, ISubscriptionRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SubscriptionRepository"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public SubscriptionRepository(DatabaseContext context)
        : base(context)
    {
    }
}