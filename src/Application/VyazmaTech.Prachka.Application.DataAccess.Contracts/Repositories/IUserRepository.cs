using VyazmaTech.Prachka.Application.Abstractions.Querying.User;
using VyazmaTech.Prachka.Domain.Core.Users;

namespace VyazmaTech.Prachka.Application.DataAccess.Contracts.Repositories;

public interface IUserRepository
{
    IAsyncEnumerable<User> QueryAsync(
        UserQuery specification,
        CancellationToken cancellationToken);

    void Insert(User user);

    Task<long> CountAsync(UserQuery specification, CancellationToken cancellationToken);
}