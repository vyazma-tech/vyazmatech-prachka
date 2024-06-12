using VyazmaTech.Prachka.Domain.Common.Errors;

namespace VyazmaTech.Prachka.Domain.Common.Exceptions;

public sealed class DomainInvalidOperationException : DomainException
{
    public DomainInvalidOperationException(Error error) : base(error) { }
}