using Domain.Common.Abstractions;

namespace Domain.Common.Errors;

public sealed class Error : ValueObject
{
    public Error(string code, string message)
    {
        Code = code;
        Message = message;
    }

    public string Code { get; }
    public string Message { get; }

    public static implicit operator string(Error error)
    {
        return error?.Code ?? string.Empty;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Code;
        yield return Message;
    }
}