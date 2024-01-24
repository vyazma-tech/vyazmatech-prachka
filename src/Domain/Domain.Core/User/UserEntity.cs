using Domain.Common.Abstractions;
using Domain.Core.User.Events;
using Domain.Kernel;

namespace Domain.Core.User;

public sealed class UserEntity : Entity, IAuditableEntity
{
    public UserEntity(
        Guid id,
        string telegramId,
        string fullname,
        DateOnly registrationDateUtc,
        SpbDateTime? modifiedOn = null)
        : base(id)
    {
        TelegramId = telegramId;
        Fullname = fullname;
        CreationDate = registrationDateUtc;
        ModifiedOn = modifiedOn;

        Raise(new UserRegisteredDomainEvent(this));
    }

    public UserInfo Info => new UserInfo(Id, TelegramId, Fullname);

    public string TelegramId { get; }

    public string Fullname { get; }

    public DateOnly CreationDate { get; }

    public SpbDateTime? ModifiedOn { get; }
}