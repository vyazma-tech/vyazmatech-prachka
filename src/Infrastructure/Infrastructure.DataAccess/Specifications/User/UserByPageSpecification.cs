using Infrastructure.DataAccess.Contracts;
using Infrastructure.DataAccess.Models;

namespace Infrastructure.DataAccess.Specifications.User;

public sealed class UserByPageSpecification : Specification<UserModel>
{
    public UserByPageSpecification(int page, int recordsPerPage)
        : base(null!)
    {
        AddPaging(page, recordsPerPage);
        AsNoTracking();
    }

    public override string ToString()
        => string.Empty;
}