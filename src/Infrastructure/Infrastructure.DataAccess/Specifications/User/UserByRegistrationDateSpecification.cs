using Domain.Common.Abstractions;
using Domain.Core.User;

namespace Infrastructure.DataAccess.Specifications.User;

public sealed class UserByRegistrationDateSpecification : Specification<UserEntity>
{
    private readonly DateTime _registrationDate;

    public UserByRegistrationDateSpecification(DateTime registrationDate)
        : base(user => user.CreationDate == registrationDate)
    {
        _registrationDate = registrationDate;
    }

    public override string ToString()
        => $"{typeof(UserEntity)}: {_registrationDate}";
}