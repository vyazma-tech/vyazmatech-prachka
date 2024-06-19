using System.Text.RegularExpressions;
using VyazmaTech.Prachka.Domain.Common.Abstractions;
using VyazmaTech.Prachka.Domain.Common.Errors;
using VyazmaTech.Prachka.Domain.Common.Exceptions;

#pragma warning disable CS8618

namespace VyazmaTech.Prachka.Domain.Core.ValueObjects;

/// <summary>
/// Describes user fullname model.
/// </summary>
public sealed class Fullname : ValueObject
{
    private const string LetterWithUppercasePattern = @"^[\p{Lu}\p{L}' \.\-]+$";

    private Fullname() { }

    private Fullname(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Gets user name.
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// Validates and creates fullname instance.
    /// </summary>
    /// <param name="name">user name.</param>
    /// <returns>constructed fullname instance.</returns>
    /// <remarks>returns failure result, when parameters didn't pass validation.</remarks>
    public static Fullname Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new UserInvalidInputException(DomainErrors.Fullname.NameIsNullOrEmpty);

        if (Regex.IsMatch(name, LetterWithUppercasePattern) is false)
            throw new UserInvalidInputException(DomainErrors.Fullname.InvalidNameFormat);

        return new Fullname(name);
    }

    public static implicit operator string(Fullname fullname) => fullname.Value;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}