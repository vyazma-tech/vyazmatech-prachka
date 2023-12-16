using Domain.Core.User;
using Domain.Kernel;

namespace Infrastructure.DataAccess.Specifications.User;

public sealed class UserByRegistrationDateSpecification : Specification<UserEntity>
{
    private readonly DateOnly _registrationDate;

    public UserByRegistrationDateSpecification(DateOnly registrationDate)
        : base(user => user.CreationDate == registrationDate)
    {
        _registrationDate = registrationDate;
    }

    public override string ToString()
        => $"{typeof(UserEntity)}: {_registrationDate}";
}