using Domain.Common.Errors;
using Domain.Common.Exceptions;
using Domain.Common.Result;
using Domain.Core.Subscription;
using Domain.Core.User;
using Infrastructure.DataAccess.Contexts;
using Infrastructure.DataAccess.Specifications.Subscription;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccess.Repositories;

internal class SubscriptionRepository : GenericRepository<SubscriptionEntity>, ISubscriptionRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SubscriptionRepository"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public SubscriptionRepository(DatabaseContext context) : base(context)
    {
    }

    public async Task<Result<SubscriptionEntity>> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        SubscriptionEntity? entity = await
            ApplySpecification(new SubscriptionByIdSpecification(id))
                .FirstOrDefaultAsync(cancellationToken);
        
        if (entity is null)
        {
            var exception = new DomainException(DomainErrors.Subscription.NotFound);
            return new Result<SubscriptionEntity>(exception);
        }

        return entity;
    }

    public async Task<Result<SubscriptionEntity>> FindByUserAsync(
        UserEntity user,
        CancellationToken cancellationToken)
    {
        SubscriptionEntity? entity = await
            ApplySpecification(new SubscriptionByUserSpecification(user))
                .FirstOrDefaultAsync(cancellationToken);
        
        if (entity is null)
        {
            var exception = new DomainException(DomainErrors.Subscription.NotFoundForRequest);
            return new Result<SubscriptionEntity>(exception);
        }

        return entity;
    }
}