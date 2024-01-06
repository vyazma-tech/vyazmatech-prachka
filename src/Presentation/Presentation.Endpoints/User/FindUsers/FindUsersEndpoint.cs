using Application.Core.Common;
using Application.Handlers.User.Queries;
using FastEndpoints;
using Mediator;

namespace Presentation.Endpoints.User.FindUsers;

// internal class FindUsersEndpoint : Endpoint<QueryConfiguration<UserQueryParameter>, PagedResponse<UserResponse>>
// {
//     private readonly IMediator _mediator;
//
//     // TODO: FIX IT
//     // private readonly IEntityQuery<UserQuery.QueryBuilder, UserQueryParameter> _query;
//     public FindUsersEndpoint(IMediator mediator)
//     {
//         _mediator = mediator;
//     }
//
//     public override void Configure()
//     {
//         Verbs(Http.POST);
//         Routes("api/users/query");
//         AllowAnonymous();
//     }
//
//     public override async Task HandleAsync(QueryConfiguration<UserQueryParameter> configuration, CancellationToken ct)
//     {
//         // UserQuery.QueryBuilder queryBuilder = UserQuery.Builder;
//         // queryBuilder = _query.Apply(queryBuilder, configuration);
//         //
//         // UserQuery userQuery = queryBuilder.Build();
//         // userQuery.Configuration = configuration;
//         //
//         // PagedResponse<UserResponse> response = await _mediator.Send(userQuery, ct);
//         await SendOkAsync(ct);
//     }
// }