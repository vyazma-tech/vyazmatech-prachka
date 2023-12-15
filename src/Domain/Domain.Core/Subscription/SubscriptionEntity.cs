using Ardalis.GuardClauses;
using Domain.Common.Abstractions;
using Domain.Common.Errors;
using Domain.Common.Exceptions;
using Domain.Common.Result;
using Domain.Core.Order;
using Domain.Core.Queue;
using Domain.Core.User;
using Domain.Kernel;

namespace Domain.Core.Subscription;

/// <summary>
/// Describes subscriber entity.
/// </summary>
public abstract class SubscriptionEntity : Entity, IAuditableEntity
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SubscriptionEntity"/> class.
    /// </summary>
    /// <param name="user">subscribed user.</param>
    /// <param name="creationDateUtc">subscription creation utc date.</param>
    public SubscriptionEntity(UserEntity user, DateOnly creationDateUtc)
        : base(Guid.NewGuid())
    {
        Guard.Against.Null(user, nameof(user), "User should not be null in subscription.");
        Guard.Against.Null(creationDateUtc, nameof(creationDateUtc), "Creation date should not be null in subscription.");

        User = user;
        CreationDate = creationDateUtc;
    }

#pragma warning disable CS8618
    protected SubscriptionEntity()
#pragma warning restore CS8618
    {
    }

    /// <summary>
    /// Gets user, who subscription is assigned to.
    /// </summary>
    public virtual UserEntity User { get; private set; }

    /// <summary>
    /// Gets subscription creation date.
    /// </summary>
    public DateOnly CreationDate { get; }

    /// <summary>
    /// Gets modification date.
    /// </summary>
    public DateTime? ModifiedOn { get; }
}