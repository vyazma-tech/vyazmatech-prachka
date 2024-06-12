using Microsoft.Extensions.Options;
using VyazmaTech.Prachka.Application.Abstractions.Configuration;
using VyazmaTech.Prachka.Application.Abstractions.Querying.User;
using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.Core.Querying.Common;
using VyazmaTech.Prachka.Application.DataAccess.Contracts;
using VyazmaTech.Prachka.Application.Dto.User;
using VyazmaTech.Prachka.Application.Mapping;
using VyazmaTech.Prachka.Domain.Core.User;
using static VyazmaTech.Prachka.Application.Contracts.Users.Queries.UserByQuery;

namespace VyazmaTech.Prachka.Application.Handlers.User.Queries;

internal sealed class UserByQueryQueryHandler : IQueryHandler<Query, Response>
{
    private readonly IPersistenceContext _persistenceContext;
    private readonly int _recordsPerPage;
    private readonly IQueryProcessor<Query, IQueryBuilder> _queryProcessor;

    public UserByQueryQueryHandler(
        IOptions<PaginationConfiguration> paginationConfiguration,
        IPersistenceContext persistenceContext,
        IQueryProcessor<Query, IQueryBuilder> queryProcessor)
    {
        _persistenceContext = persistenceContext;
        _queryProcessor = queryProcessor;
        _recordsPerPage = paginationConfiguration.Value.RecordsPerPage;
    }

    public async ValueTask<Response> Handle(Query request, CancellationToken cancellationToken)
    {
        IQueryBuilder builder = _queryProcessor.Process(request, UserQuery.Builder());
        UserQuery query = builder.Build();

        long totalCount = await _persistenceContext.Users.CountAsync(query, cancellationToken);

        List<UserEntity> users = await _persistenceContext.Users
            .QueryAsync(query, cancellationToken)
            .ToListAsync(cancellationToken);

        IEnumerable<UserDto> result = users.Select(x => x.ToDto());

        var page = result.ToPagedResponse(
            query.Page + 1 ?? 1,
            recordsPerPage: _recordsPerPage,
            totalPages: (totalCount / _recordsPerPage) + 1);

        return new Response(page);
    }
}