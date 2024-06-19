using VyazmaTech.Prachka.Domain.Common.Abstractions;

namespace VyazmaTech.Prachka.Domain.Common.Errors;

public sealed class Error : ValueObject
{
    private Error(string code, string message, ErrorType type)
    {
        Code = code;
        Message = message;
        Type = type;
    }

    public string Code { get; }

    public string Message { get; }

    public ErrorType Type { get; }

    public static Error NotFound(string code, string description)
    {
        return new Error(code, description, ErrorType.NotFound);
    }

    public static Error BadRequest(string code, string description)
    {
        return new Error(code, description, ErrorType.BadRequest);
    }

    public static Error Conflict(string code, string description)
    {
        return new Error(code, description, ErrorType.Conflict);
    }

    public static Error Unprocessable(string code, string description)
    {
        return new Error(code, description, ErrorType.Unprocessable);
    }

    public static Error Validation(string code, string description)
    {
        return new Error(code, description, ErrorType.Validation);
    }

    public static Error Failure(string code, string description)
    {
        return new Error(code, description, ErrorType.Failure);
    }

    public static Error Unauthorized(string code, string description)
    {
        return new Error(code, description, ErrorType.Unauthorized);
    }

    public static Error Forbidden(string code, string description)
    {
        return new Error(code, description, ErrorType.Forbidden);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Code;
        yield return Message;
        yield return Type;
    }
}