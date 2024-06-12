using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.Core.Specifications;
using VyazmaTech.Prachka.Application.DataAccess.Contracts;
using VyazmaTech.Prachka.Application.Mapping;
using VyazmaTech.Prachka.Domain.Common.Result;
using VyazmaTech.Prachka.Domain.Core.User;
using static VyazmaTech.Prachka.Application.Contracts.Users.Queries.UserById;

namespace VyazmaTech.Prachka.Application.Handlers.User.Queries;

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
            .FindByIdAsync(request.Id, cancellationToken);

        if (result.IsFaulted)
        {
            return new Result<Response>(result.Error);
        }

        UserEntity user = result.Value;

        return new Response(user.ToDto());
    }
}