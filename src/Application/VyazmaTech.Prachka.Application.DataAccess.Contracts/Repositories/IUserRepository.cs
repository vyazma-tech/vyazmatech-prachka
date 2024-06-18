using VyazmaTech.Prachka.Application.Contracts.Users.Queries;
using VyazmaTech.Prachka.Domain.Core.Users;

namespace VyazmaTech.Prachka.Application.DataAccess.Contracts.Repositories;

public interface IUserRepository
{
    Task<User> GetByIdAsync(Guid id, CancellationToken token);

    IAsyncEnumerable<User> QueryAsync(
        UserByQuery.Query specification,
        CancellationToken cancellationToken);

    void Insert(User user);

    Task<long> CountAsync(UserByQuery.Query specification, CancellationToken cancellationToken);
}