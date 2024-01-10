using Infrastructure.DataAccess.Contracts;
using Infrastructure.DataAccess.Models;

namespace Infrastructure.DataAccess.Specifications.User;

public sealed class UserByIdSpecification : Specification<UserModel>
{
    private readonly Guid _id;

    public UserByIdSpecification(Guid id)
        : base(user => user.Id == id)
    {
        _id = id;
    }

    public override string ToString()
        => $"UserId = {_id}";
}