using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.Core.Specifications;
using VyazmaTech.Prachka.Application.DataAccess.Contracts;
using VyazmaTech.Prachka.Application.Mapping;
using static VyazmaTech.Prachka.Application.Contracts.Users.Queries.UserById;

namespace VyazmaTech.Prachka.Application.Handlers.User.Queries;

internal sealed class UserByIdQueryHandler : IQueryHandler<Query, Response>
{
    private readonly IPersistenceContext _persistenceContext;

    public UserByIdQueryHandler(IPersistenceContext persistenceContext)
    {
        _persistenceContext = persistenceContext;
    }

    public async ValueTask<Response> Handle(Query request, CancellationToken cancellationToken)
    {
        Domain.Core.Users.User user = await _persistenceContext.Users
            .FindByIdAsync(request.Id, cancellationToken);

        return new Response(user.ToDto());
    }
}