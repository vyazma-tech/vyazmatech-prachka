using Microsoft.Extensions.Options;
using VyazmaTech.Prachka.Application.Abstractions.Configuration;
using VyazmaTech.Prachka.Application.Abstractions.Querying.User;
using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.DataAccess.Contracts;
using VyazmaTech.Prachka.Application.Dto.User;
using VyazmaTech.Prachka.Application.Mapping;
using static VyazmaTech.Prachka.Application.Contracts.Users.Queries.UserByQuery;

namespace VyazmaTech.Prachka.Application.Handlers.User.Queries;

internal sealed class UserByQueryQueryHandler : IQueryHandler<Query, Response>
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

    public async ValueTask<Response> Handle(Query request, CancellationToken cancellationToken)
    {
        UserQuery query = BuildQuery(request);

        long totalCount = await _persistenceContext.Users.CountAsync(query, cancellationToken);

        List<UserDto> users = await _persistenceContext.Users
            .QueryAsync(query, cancellationToken)
            .Select(x => x.ToDto())
            .ToListAsync(cancellationToken);

        var page = users.ToPagedResponse(
            query.Page,
            recordsPerPage: _recordsPerPage,
            totalPages: (totalCount / _recordsPerPage) + 1);

        return new Response(page);
    }

    private UserQuery BuildQuery(Query request)
    {
        IQueryBuilder builder = UserQuery.Builder()
            .WithLimit(_recordsPerPage)
            .WithPage(request.Page);

        if (request.RegistrationDate is not null)
            builder = builder.WithRegistrationDate(request.RegistrationDate.Value);

        if (request.Fullname is not null)
            builder = builder.WithFullname(request.Fullname);

        if (request.TelegramUsername is not null)
            builder = builder.WithTelegramUsername(request.TelegramUsername);

        return builder.Build();
    }
}