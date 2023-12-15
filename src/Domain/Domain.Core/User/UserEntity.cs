using Ardalis.GuardClauses;
using Domain.Common.Abstractions;
using Domain.Core.User.Events;
using Domain.Core.ValueObjects;
using Domain.Kernel;

namespace Domain.Core.User;

/// <summary>
/// Describes user entity.
/// </summary>
public class UserEntity : Entity, IAuditableEntity
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserEntity" /> class.
    /// </summary>
    /// <param name="telegramId">user telegram id.</param>
    /// <param name="fullname">user full name.</param>
    /// <param name="registrationDateUtc">user registration date.</param>
    public UserEntity(TelegramId telegramId, Fullname fullname, DateOnly registrationDateUtc)
        : base(Guid.NewGuid())
    {
        Guard.Against.Null(telegramId, nameof(telegramId), "Telegram ID should not be null.");
        Guard.Against.Null(registrationDateUtc, nameof(registrationDateUtc), "Creation date should not be null.");
        Guard.Against.Null(fullname, nameof(fullname), "Name should not be null.");

        TelegramId = telegramId;
        Fullname = fullname;
        CreationDate = registrationDateUtc;
        ModifiedOn = null;

        Raise(new UserRegisteredDomainEvent(this));
    }

#pragma warning disable CS8618
    protected UserEntity() { }
#pragma warning restore CS8618

    /// <summary>
    /// Gets telegram id.
    /// </summary>
    public TelegramId TelegramId { get; }

    /// <summary>
    /// Gets telegram id.
    /// </summary>
    public Fullname Fullname { get; }

    /// <summary>
    /// Gets registration date.
    /// </summary>
    public DateOnly CreationDate { get; }

    /// <summary>
    /// Gets modification date.
    /// </summary>
    public DateTime? ModifiedOn { get; }
}