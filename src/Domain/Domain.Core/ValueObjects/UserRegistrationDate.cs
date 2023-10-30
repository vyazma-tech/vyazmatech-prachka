using Domain.Common.Abstractions;
using Domain.Common.Errors;
using Domain.Common.Exceptions;
using LanguageExt.Common;

namespace Domain.Core.ValueObjects;

public sealed class UserRegistrationDate : ValueObject
{
    private UserRegistrationDate(DateTime value)
    {
        Value = value;
    }

    public DateTime Value { get; }

    public static Result<UserRegistrationDate> Create(DateTime dateTimeUtc, IDateTimeProvider dateTimeProvider)
    {
        if (dateTimeUtc < dateTimeProvider.UtcNow)
        {
            var exception = new DomainException(DomainErrors.UserRegistrationDate.InThePast);
            return new Result<UserRegistrationDate>(exception);
        }

        return new UserRegistrationDate(dateTimeUtc);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}