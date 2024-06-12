﻿using VyazmaTech.Prachka.Domain.Common.Abstractions;
using VyazmaTech.Prachka.Domain.Core.Users.Events;
using VyazmaTech.Prachka.Domain.Core.ValueObjects;
using VyazmaTech.Prachka.Domain.Kernel;

#pragma warning disable CS8618

namespace VyazmaTech.Prachka.Domain.Core.Users;

public sealed class User : Entity, IAuditableEntity
{
    private User() : base() { }

    private User(
        Guid id,
        string telegramUsername,
        Fullname fullname,
        DateOnly registrationDate,
        DateTime? modifiedOn = null)
        : base(id)
    {
        TelegramUsername = telegramUsername;
        Fullname = fullname;
        CreationDate = registrationDate;
        ModifiedOnUtc = modifiedOn;
    }

    public string TelegramUsername { get; }

    public Fullname Fullname { get; }

    public DateOnly CreationDate { get; }

    public DateTime? ModifiedOnUtc { get; set; }

    public static User Create(Guid id, string telegramUsername, Fullname fullname, DateOnly registrationDate)
    {
        var user = new User(id, telegramUsername, fullname, registrationDate);

        user.Raise(new UserRegisteredDomainEvent(user));

        return user;
    }
}