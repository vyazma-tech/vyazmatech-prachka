using Ardalis.GuardClauses;
using Domain.Common.Abstractions;
using Domain.Core.User.Events;
using Domain.Core.ValueObjects;

namespace Domain.Core.User;

public sealed class UserEntity : Entity, IAuditableEntity
{
    public UserEntity(TelegramId telegramId, DateTime registrationDateUtc)
        : base(Guid.NewGuid())
    {
        Guard.Against.Null(telegramId, nameof(telegramId), "Telegram ID should not be null.");
        Guard.Against.Null(registrationDateUtc, nameof(registrationDateUtc), "Creation date should not be null.");

        TelegramId = telegramId;
        CreationDate = registrationDateUtc;
        ModifiedOn = null;

        Raise(new UserRegisteredDomainEvent(this));
    }

#pragma warning disable CS8618
    private UserEntity() { }
#pragma warning restore CS8618

    public TelegramId TelegramId { get; }
    public DateTime CreationDate { get; }
    public DateTime? ModifiedOn { get; }
}