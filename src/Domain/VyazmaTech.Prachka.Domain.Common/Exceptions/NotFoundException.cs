using VyazmaTech.Prachka.Domain.Common.Errors;

namespace VyazmaTech.Prachka.Domain.Common.Exceptions;

public sealed class NotFoundException : DomainException
{
    public NotFoundException(Error error) : base(error) { }

    public NotFoundException(Error error, Exception? innerException) : base(error, innerException) { }
}