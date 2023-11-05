using System.Text.RegularExpressions;
using Domain.Common.Abstractions;
using Domain.Common.Errors;
using Domain.Common.Exceptions;
using Domain.Common.Result;

namespace Domain.Core.ValueObjects;

/// <summary>
/// Describes user fullname model.
/// </summary>
public sealed class Fullname : ValueObject
{
    private const string LetterWithUppercasePattern = @"^\p{Lu}{1}\p{Ll}*$";

    private Fullname(string firstname, string middlename, string lastname)
    {
        Firstname = firstname;
        Middlename = middlename;
        Lastname = lastname;
    }

    /// <summary>
    /// Gets user firstname.
    /// </summary>
    public string Firstname { get; }

    /// <summary>
    /// Gets user middlename.
    /// </summary>
    public string Middlename { get; }

    /// <summary>
    /// Gets user lastname.
    /// </summary>
    public string Lastname { get; }

    /// <summary>
    /// Validates and creates fullname instance.
    /// </summary>
    /// <param name="firstname">user firstname.</param>
    /// <param name="middlename">user middlename.</param>
    /// <param name="lastname">user lastname.</param>
    /// <returns>constructed fullname instance.</returns>
    /// <remarks>returns failure result, when parameters didn't pass validation.</remarks>
    public static Result<Fullname> Create(string firstname, string middlename, string lastname)
    {
        if (string.IsNullOrWhiteSpace(firstname))
        {
            var exception = new DomainException(DomainErrors.Fullname.FirstnameIsNullOrEmpty);
            return new Result<Fullname>(exception);
        }

        if (string.IsNullOrWhiteSpace(middlename))
        {
            var exception = new DomainException(DomainErrors.Fullname.MiddlenameIsNullOrEmpty);
            return new Result<Fullname>(exception);
        }

        if (string.IsNullOrWhiteSpace(lastname))
        {
            var exception = new DomainException(DomainErrors.Fullname.LastnameIsNullOrEmpty);
            return new Result<Fullname>(exception);
        }

        if (Regex.IsMatch(firstname, LetterWithUppercasePattern) is false)
        {
            var exception = new DomainException(DomainErrors.Fullname.InvalidFirstnameFormat);
            return new Result<Fullname>(exception);
        }

        if (Regex.IsMatch(middlename, LetterWithUppercasePattern) is false)
        {
            var exception = new DomainException(DomainErrors.Fullname.InvalidMiddlenameFormat);
            return new Result<Fullname>(exception);
        }

        if (Regex.IsMatch(lastname, LetterWithUppercasePattern) is false)
        {
            var exception = new DomainException(DomainErrors.Fullname.InvalidLastnameFormat);
            return new Result<Fullname>(exception);
        }

        return new Fullname(firstname, middlename, lastname);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}