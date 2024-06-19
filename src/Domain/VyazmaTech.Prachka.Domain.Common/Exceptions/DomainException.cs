using VyazmaTech.Prachka.Domain.Common.Errors;

namespace VyazmaTech.Prachka.Domain.Common.Exceptions;

public abstract class DomainException : Exception
{
    protected DomainException(Error error) : base(ToMessage(error))
    {
        Error = error;
    }

    protected DomainException(Error error, Exception? innerException) : base(ToMessage(error), innerException)
    {
        Error = error;
    }

    public Error Error { get; private set; }

    private static string ToMessage(Error error)
        => $"[{error.Type}]: {error.Message}";
}