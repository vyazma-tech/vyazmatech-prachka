using Domain.Common.Abstractions;
using Domain.Core.User.Events;
using Domain.Core.ValueObjects;

namespace Domain.Core.User;

public sealed class UserEntity : Entity, IAuditableEntity
{
    public UserEntity(TelegramId telegramId, UserRegistrationDate creationDate)
        : base(Guid.NewGuid())
    {
        TelegramId = telegramId;
        CreationDate = creationDate.Value;
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