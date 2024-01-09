using Application.DataAccess.Contracts;
using Domain.Core.User;
using Domain.Core.ValueObjects;
using Domain.Kernel;
using Infrastructure.DataAccess.Contracts;

namespace Infrastructure.DataAccess.Specifications.User;

public sealed class UserByFullnameSpecification : Specification<UserEntity>
{
    private readonly Fullname _fullname;

    public UserByFullnameSpecification(Fullname fullname)
        : base(user => user.Fullname == fullname)
    {
        _fullname = fullname;
    }

    public override string ToString()
        => $"{typeof(UserEntity)}: {_fullname}";
}