using VyazmaTech.Prachka.Application.Abstractions.Querying.User;
using VyazmaTech.Prachka.Application.DataAccess.Contracts.Repositories;
using VyazmaTech.Prachka.Domain.Common.Errors;
using VyazmaTech.Prachka.Domain.Common.Exceptions;
using VyazmaTech.Prachka.Domain.Core.Users;

namespace VyazmaTech.Prachka.Application.Core.Specifications;

public static class UserSpecifications
{
    public static async Task<User> FindByIdAsync(
        this IUserRepository repository,
        Guid id,
        CancellationToken token)
    {
        var query = UserQuery.Build(x => x.WithId(id));

        User? result = await repository.QueryAsync(query, token).SingleOrDefaultAsync(token);

        if (result is null)
            throw new NotFoundException(DomainErrors.Entity.NotFoundFor<User>(id.ToString()));

        return result;
    }
}