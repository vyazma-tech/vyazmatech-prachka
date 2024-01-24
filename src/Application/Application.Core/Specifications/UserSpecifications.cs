using Application.DataAccess.Contracts.Querying.User;
using Application.DataAccess.Contracts.Repositories;
using Domain.Common.Errors;
using Domain.Common.Result;
using Domain.Core.User;

namespace Application.Core.Specifications;

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