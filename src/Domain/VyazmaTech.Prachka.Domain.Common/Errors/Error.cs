using VyazmaTech.Prachka.Domain.Common.Abstractions;

namespace VyazmaTech.Prachka.Domain.Common.Errors;

public sealed class Error : ValueObject
{
    private Error(string code, string message, ErrorType type, ErrorArea area)
    {
        Code = code;
        Message = message;
        Type = type;
        Area = area;
    }

    public string Code { get; }

    public string Message { get; }

    public ErrorType Type { get; }

    public ErrorArea Area { get; }

    public static Error NotFound(string code, string description, ErrorArea area)
    {
        return new Error(code, description, ErrorType.NotFound, area);
    }

    public static Error BadRequest(string code, string description, ErrorArea area)
    {
        return new Error(code, description, ErrorType.BadRequest, area);
    }

    public static Error Conflict(string code, string description, ErrorArea area)
    {
        return new Error(code, description, ErrorType.Conflict, area);
    }

    public static Error Unprocessable(string code, string description, ErrorArea area)
    {
        return new Error(code, description, ErrorType.Unprocessable, area);
    }

    public static Error Validation(string code, string description, ErrorArea area)
    {
        return new Error(code, description, ErrorType.Validation, area);
    }

    public static Error Failure(string code, string description, ErrorArea area)
    {
        return new Error(code, description, ErrorType.Failure, area);
    }

    public static Error Unauthorized(string code, string description, ErrorArea area)
    {
        return new Error(code, description, ErrorType.Unauthorized, area);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Code;
        yield return Message;
        yield return Type;
        yield return Area;
    }
}