using VyazmaTech.Prachka.Domain.Common.Errors;

namespace VyazmaTech.Prachka.Domain.Common.Exceptions;

public sealed class UserInvalidInputException : DomainException
{
    public UserInvalidInputException(Error error) : base(error) { }
}