using System.Text.RegularExpressions;
using Domain.Common.Abstractions;
using Domain.Common.Errors;
using Domain.Common.Result;

namespace Domain.Core.ValueObjects;

/// <summary>
/// Describes user fullname model.
/// </summary>
public sealed class Fullname : ValueObject
{
    private const string LetterWithUppercasePattern = @"^[\p{Lu}\p{L}' \.\-]+$";

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
    public static Result<Fullname> Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return new Result<Fullname>(DomainErrors.Fullname.NameIsNullOrEmpty);
        }

        if (Regex.IsMatch(name, LetterWithUppercasePattern) is false)
        {
            return new Result<Fullname>(DomainErrors.Fullname.InvalidNameFormat);
        }

        return new Fullname(name);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}