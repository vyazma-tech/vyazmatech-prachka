using VyazmaTech.Prachka.Domain.Common.Errors;

namespace VyazmaTech.Prachka.Domain.Common.Exceptions;

public sealed class IdentityException : DomainException
{
    public IdentityException(Error error) : base(error) { }

    public IdentityException(Error error, Exception? innerException) : base(error, innerException) { }
}