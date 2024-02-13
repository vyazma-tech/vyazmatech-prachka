using VyazmaTech.Prachka.Application.Abstractions.Querying.User;
using VyazmaTech.Prachka.Domain.Core.User;

namespace VyazmaTech.Prachka.Application.DataAccess.Contracts.Repositories;

public interface IUserRepository
{
    IAsyncEnumerable<UserEntity> QueryAsync(
        UserQuery specification,
        CancellationToken cancellationToken);

    void Insert(UserEntity user);

    Task<long> CountAsync(UserQuery specification, CancellationToken cancellationToken);
}