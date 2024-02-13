using VyazmaTech.Prachka.Application.Abstractions.Querying.User;
using VyazmaTech.Prachka.Application.DataAccess.Contracts.Repositories;
using VyazmaTech.Prachka.Domain.Common.Errors;
using VyazmaTech.Prachka.Domain.Common.Result;
using VyazmaTech.Prachka.Domain.Core.User;

namespace VyazmaTech.Prachka.Application.Core.Specifications;

public static class UserSpecifications
{
    public static async Task<Result<UserEntity>> FindByIdAsync(
        this IUserRepository repository,
        Guid id,
        CancellationToken token)
    {
        var query = UserQuery.Build(x => x.WithId(id));

        UserEntity? result = await repository.QueryAsync(query, token).SingleOrDefaultAsync(token);

        if (result is null)
        {
            return new Result<UserEntity>(DomainErrors.Entity.NotFoundFor<UserEntity>(id.ToString()));
        }

        return result;
    }
}