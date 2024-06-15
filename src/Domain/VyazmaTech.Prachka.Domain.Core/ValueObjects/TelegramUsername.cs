using VyazmaTech.Prachka.Domain.Common.Abstractions;
using VyazmaTech.Prachka.Domain.Common.Errors;
using VyazmaTech.Prachka.Domain.Common.Exceptions;

#pragma warning disable CS8618

namespace VyazmaTech.Prachka.Domain.Core.ValueObjects;

/// <summary>
/// Describes telegram username model.
/// </summary>
public sealed class TelegramUsername : ValueObject
{
    private TelegramUsername() { }

    private TelegramUsername(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Gets telegram id.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Validates and creates telegram id instance.
    /// </summary>
    /// <param name="telegramUsername">telegram id.</param>
    /// <returns>constructed telegram id instance.</returns>
    /// <remarks>returns failure result, when parameter didn't pass validation.</remarks>
    public static TelegramUsername Create(string telegramUsername)
    {
        if (string.IsNullOrWhiteSpace(telegramUsername))
            throw new UserInvalidInputException(DomainErrors.TelegramId.NullOrEmpty);

        if (telegramUsername.First() is not '@')
            throw new UserInvalidInputException(DomainErrors.TelegramId.InvalidFormat);

        return new TelegramUsername(telegramUsername);
    }

    public static implicit operator string(TelegramUsername telegramUsername) => telegramUsername.Value;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}