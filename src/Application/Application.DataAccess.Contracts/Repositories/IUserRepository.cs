using Application.DataAccess.Contracts.Querying.User;
using Domain.Core.User;

namespace Application.DataAccess.Contracts.Repositories;

public interface IUserRepository
{
    IAsyncEnumerable<UserEntity> QueryAsync(
        UserQuery specification,
        CancellationToken cancellationToken);

    void Insert(UserEntity user);

    Task<long> CountAsync(UserQuery specification, CancellationToken cancellationToken);
}