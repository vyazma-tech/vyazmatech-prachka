using System.Text.RegularExpressions;
using Domain.Common.Abstractions;
using Domain.Common.Errors;
using Domain.Common.Exceptions;
using LanguageExt.Common;

namespace Domain.Core.ValueObjects;

public sealed class TelegramId : ValueObject
{
    private const string DigitNumberPattern = @"^\d*$";

    private TelegramId(string value)
        => Value = value;

    public string Value { get; }

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