using Application.Core.Common;
using Application.Core.Configuration;
using Application.Core.Contracts.Common;
using Application.Core.Querying.Common;
using Application.DataAccess.Contracts;
using Application.DataAccess.Contracts.Querying.User;
using Domain.Core.User;
using Microsoft.Extensions.Options;
using static Application.Core.Contracts.Users.Queries.UserByQuery;

namespace Application.Handlers.User.Queries;

internal sealed class UserByQueryQueryHandler : IQueryHandler<Query, PagedResponse<Response>>
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

    public async ValueTask<PagedResponse<Response>> Handle(Query request, CancellationToken cancellationToken)
    {
        IQueryBuilder builder = _queryProcessor.Process(request, UserQuery.Builder());
        UserQuery query = builder.Build();

        long totalCount = await _persistenceContext.Users.CountAsync(query, cancellationToken);

        List<UserEntity> users = await _persistenceContext.Users
            .QueryAsync(query, cancellationToken)
            .ToListAsync(cancellationToken);

        Response[] result = users.Select(x => x.ToDto()).ToArray();

        return new PagedResponse<Response>
        {
            Bunch = result,
            CurrentPage = query.Page + 1 ?? 1,
            RecordPerPage = _recordsPerPage,
            TotalPages = (totalCount / _recordsPerPage) + 1
        };
    }
}