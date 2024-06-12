using VyazmaTech.Prachka.Domain.Common.Abstractions;
using VyazmaTech.Prachka.Domain.Core.User.Events;
using VyazmaTech.Prachka.Domain.Kernel;

namespace VyazmaTech.Prachka.Domain.Core.User;

public sealed class UserEntity : Entity, IAuditableEntity
{
    public UserEntity(
        Guid id,
        string telegramUsername,
        string fullname,
        DateOnly registrationDateUtc,
        DateTime? modifiedOn = null)
        : base(id)
    {
        TelegramUsername = telegramUsername;
        Fullname = fullname;
        CreationDate = registrationDateUtc;
        ModifiedOnUtc = modifiedOn;

        Raise(new UserRegisteredDomainEvent(this));
    }

    public UserInfo Info => new(Id, TelegramUsername, Fullname);

    public string TelegramUsername { get; }

    public string Fullname { get; }

    public DateOnly CreationDate { get; }

    public DateTime? ModifiedOnUtc { get; }
}