using System.Text.RegularExpressions;
using VyazmaTech.Prachka.Domain.Common.Abstractions;
using VyazmaTech.Prachka.Domain.Common.Errors;
using VyazmaTech.Prachka.Domain.Common.Exceptions;

namespace VyazmaTech.Prachka.Domain.Core.ValueObjects;

/// <summary>
/// Describes telegram id model.
/// </summary>
public sealed class TelegramId : ValueObject
{
    private const string DigitNumberPattern = @"^\d*$";

    private TelegramId(string value)
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
    /// <param name="telegramId">telegram id.</param>
    /// <returns>constructed telegram id instance.</returns>
    /// <remarks>returns failure result, when parameter didn't pass validation.</remarks>
    public static TelegramId Create(string telegramId)
    {
        if (string.IsNullOrWhiteSpace(telegramId))
            throw new UserInvalidInputException(DomainErrors.TelegramId.NullOrEmpty);

        if (Regex.IsMatch(telegramId, DigitNumberPattern) is false)
            throw new UserInvalidInputException(DomainErrors.TelegramId.InvalidFormat);

        return new TelegramId(telegramId);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}