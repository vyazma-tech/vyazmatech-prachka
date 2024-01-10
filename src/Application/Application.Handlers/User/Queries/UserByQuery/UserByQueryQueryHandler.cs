using Application.Core.Common;
using Application.Core.Configuration;
using Application.Core.Contracts;
using Application.Core.Extensions;
using Domain.Core.User;
using Infrastructure.DataAccess.Contracts;
using Infrastructure.DataAccess.Specifications.User;
using Microsoft.Extensions.Options;
using static Application.Handlers.User.Queries.UserByQuery.UserByQueryQuery;

namespace Application.Handlers.User.Queries.UserByQuery;

internal sealed class UserByQueryQueryHandler : IQueryHandler<Query, PagedResponse<Response>>
{
    private readonly IPersistenceContext _persistenceContext;
    private readonly int _recordsPerPage;

    public UserByQueryQueryHandler(
        IOptions<PaginationConfiguration> paginationConfiguration,
        IPersistenceContext persistenceContext)
    {
        _persistenceContext = persistenceContext;
        _recordsPerPage = paginationConfiguration.Value.RecordsPerPage;
    }

    public async ValueTask<PagedResponse<Response>> Handle(Query request, CancellationToken cancellationToken)
    {
        var specification = new UserByPageSpecification(request.Page, _recordsPerPage);
        long totalCount = await _persistenceContext.Users.CountAsync(specification, cancellationToken);

        List<UserEntity> users = await _persistenceContext.Users
            .QueryAsync(specification, cancellationToken)
            .ToListAsync(cancellationToken);

        Response[] result = users
            .FilterBy(
                request.TelegramId,
                request.Fullname,
                request.RegistrationDate)
            .Select(x => x.ToDto())
            .ToArray();

        return new PagedResponse<Response>
        {
            Bunch = result,
            CurrentPage = request.Page + 1,
            RecordPerPage = _recordsPerPage,
            TotalPages = (totalCount / _recordsPerPage) + 1,
        };
    }
}