using System.Text.RegularExpressions;
using Domain.Common.Abstractions;
using Domain.Common.Errors;
using Domain.Common.Exceptions;
using Domain.Common.Result;

namespace Domain.Core.ValueObjects;

/// <summary>
/// Describes telegram id model.
/// </summary>
public sealed class TelegramId : ValueObject
{
    private const string DigitNumberPattern = @"^\d*$";

    private TelegramId(string value)
        => Value = value;

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
    public static Result<TelegramId> Create(string telegramId)
    {
        if (string.IsNullOrWhiteSpace(telegramId))
        {
            var exception = new DomainException(DomainErrors.TelegramId.NullOrEmpty);
            return new Result<TelegramId>(exception);
        }

        if (Regex.IsMatch(telegramId, DigitNumberPattern) is false)
        {
            var exception = new DomainException(DomainErrors.TelegramId.InvalidFormat);
            return new Result<TelegramId>(exception);
        }

        return new TelegramId(telegramId);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}