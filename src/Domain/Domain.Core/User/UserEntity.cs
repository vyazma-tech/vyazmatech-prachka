using Ardalis.GuardClauses;
using Domain.Common.Abstractions;
using Domain.Core.User.Events;
using Domain.Core.ValueObjects;
using Domain.Kernel;

namespace Domain.Core.User;

/// <summary>
/// Describes user entity.
/// </summary>
public sealed class UserEntity : Entity, IAuditableEntity
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserEntity" /> class.
    /// </summary>
    /// <param name="id">user id.</param>
    /// <param name="telegramId">user telegram id.</param>
    /// <param name="fullname">user full name.</param>
    /// <param name="registrationDateUtc">user registration date.</param>
    /// <param name="modifiedOn">user modification date.</param>
    public UserEntity(
        Guid id,
        TelegramId telegramId,
        Fullname fullname,
        DateOnly registrationDateUtc,
        SpbDateTime? modifiedOn = null)
        : base(id)
    {
        Guard.Against.Null(telegramId, nameof(telegramId), "Telegram ID should not be null.");
        Guard.Against.Null(registrationDateUtc, nameof(registrationDateUtc), "Creation date should not be null.");
        Guard.Against.Null(fullname, nameof(fullname), "Name should not be null.");

        TelegramId = telegramId;
        Fullname = fullname;
        CreationDate = registrationDateUtc;
        ModifiedOn = modifiedOn;

        Raise(new UserRegisteredDomainEvent(this));
    }

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
    public SpbDateTime? ModifiedOn { get; }
}