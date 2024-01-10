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
    /// <param name="id">subscription id.</param>
    /// <param name="user">subscribed user.</param>
    /// <param name="creationDateUtc">subscription creation utc date.</param>
    /// <param name="modifiedOn">subscription modification date.</param>
    public SubscriptionEntity(
        Guid id,
        UserEntity user,
        DateOnly creationDateUtc,
        DateTime? modifiedOn = null)
        : base(id)
    {
        Guard.Against.Null(user, nameof(user), "User should not be null in subscription.");
        Guard.Against.Null(creationDateUtc, nameof(creationDateUtc), "Creation date should not be null in subscription.");

        User = user;
        CreationDate = creationDateUtc;
        ModifiedOn = modifiedOn;
    }

    /// <summary>
    /// Gets user, who subscription is assigned to.
    /// </summary>
    public UserEntity User { get; private set; }

    /// <summary>
    /// Gets subscription creation date.
    /// </summary>
    public DateOnly CreationDate { get; }

    /// <summary>
    /// Gets modification date.
    /// </summary>
    public DateTime? ModifiedOn { get; }
}