using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.DataAccess.Contracts;
using VyazmaTech.Prachka.Application.Dto.User;
using VyazmaTech.Prachka.Application.Mapping;
using static VyazmaTech.Prachka.Application.Contracts.Users.Queries.UserByQuery;

namespace VyazmaTech.Prachka.Application.Handlers.User.Queries;

internal sealed class UserByQueryQueryHandler : IQueryHandler<Query, Response>
{
    private readonly IPersistenceContext _persistenceContext;

    public UserByQueryQueryHandler(IPersistenceContext persistenceContext)
    {
        _persistenceContext = persistenceContext;
    }

    public async ValueTask<Response> Handle(Query request, CancellationToken cancellationToken)
    {
        long totalCount = await _persistenceContext.Users.CountAsync(request, cancellationToken);

        List<UserDto> users = await _persistenceContext.Users
            .QueryAsync(request, cancellationToken)
            .Select(x => x.ToDto())
            .ToListAsync(cancellationToken);

        var page = users.ToPagedResponse(
            request.Page,
            recordsPerPage: request.Limit,
            totalPages: (totalCount / request.Limit) + 1);

        return new Response(page);
    }
}