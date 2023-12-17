using Application.Core.Common;
using Application.Handlers.Order.Queries;
using Application.Handlers.User.Queries;
using FastEndpoints;
using Infrastructure.DataAccess.Quering.Abstractions;
using Mediator;

namespace Presentation.Endpoints.User.FindUsers;

internal class FindUsersEndpoint : Endpoint<QueryConfiguration<UserQueryParameter>, PagedResponse<UserResponse>>
{
    private readonly IMediator _mediator;
    private readonly IModelQuery<UserQuery.QueryBuilder, UserQueryParameter> _query;

    public FindUsersEndpoint(IMediator mediator, IModelQuery<UserQuery.QueryBuilder, UserQueryParameter> query)
    {
        _mediator = mediator;
        _query = query;
    }

    public override void Configure()
    {
        Verbs(Http.POST);
        Routes("api/users/query");
        AllowAnonymous();
    }

    public override async Task HandleAsync(QueryConfiguration<UserQueryParameter> configuration, CancellationToken ct)
    {
        UserQuery.QueryBuilder queryBuilder = UserQuery.Builder;
        queryBuilder = _query.Apply(queryBuilder, configuration);

        UserQuery userQuery = queryBuilder.Build();

        PagedResponse<UserResponse> response = await _mediator.Send(userQuery, ct);
        await SendOkAsync(response, ct);
    }
}