using Domain.Core.User;

namespace Infrastructure.DataAccess.Specifications.User;

public sealed class UserByIdSpecification : Specification<UserEntity>
{
    private readonly Guid _id;
    
    public UserByIdSpecification(Guid id)
        : base(user => user.Id == id)
    {
        _id = id;
    }

    public override string ToString()
        => $"{typeof(Guid)}: {_id}";
}