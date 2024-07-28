using VyazmaTech.Prachka.Domain.Common.Abstractions;
using VyazmaTech.Prachka.Domain.Common.Errors;
using VyazmaTech.Prachka.Domain.Common.Exceptions;

namespace VyazmaTech.Prachka.Domain.Core.ValueObjects;

/// <summary>
/// Order price
/// </summary>
public sealed class Price : ValueObject
{
    private Price() { }

    private Price(double price)
    {
        Value = price;
    }

    public double Value { get; }

    public static Price Create(double? price)
    {
        if (price < 0 || price is null)
            throw new UserInvalidInputException(DomainErrors.Price.NegativePrice);

        return new Price(price.Value);
    }

    public static Price Default => new(0);

    public static implicit operator double(Price price) => price.Value;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}