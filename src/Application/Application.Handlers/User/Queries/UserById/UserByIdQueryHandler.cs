using Application.Core.Contracts;
using Domain.Common.Result;
using Domain.Core.User;
using Infrastructure.DataAccess.Contracts;
using Infrastructure.DataAccess.Specifications.User;
using static Application.Handlers.User.Queries.UserById.UserByIdQuery;

namespace Application.Handlers.User.Queries.UserById;

internal sealed class UserByIdQueryHandler : IQueryHandler<Query, Result<Response>>
{
    private readonly IPersistenceContext _persistenceContext;

    public UserByIdQueryHandler(IPersistenceContext persistenceContext)
    {
        _persistenceContext = persistenceContext;
    }

    public async ValueTask<Result<Response>> Handle(Query request, CancellationToken cancellationToken)
    {
        Result<UserEntity> result = await _persistenceContext.Users
            .FindByAsync(
                new UserByIdSpecification(request.Id),
                cancellationToken);

        if (result.IsFaulted)
            return new Result<Response>(result.Error);

        UserEntity user = result.Value;

        return user.ToDto();
    }
}