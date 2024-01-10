using Application.Core.Common;
using FastEndpoints;
using Mediator;
using Presentation.Endpoints.Extensions;
using static Application.Handlers.User.Queries.UserByQuery.UserByQueryQuery;

namespace Presentation.Endpoints.User.FindUsers;

internal class FindUsersEndpoint : Endpoint<Query, PagedResponse<Response>>
{
    private readonly IMediator _mediator;

    public FindUsersEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Verbs(Http.POST);
        Routes("api/users/query");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Query req, CancellationToken ct)
    {
        PagedResponse<Response> response = await _mediator.Send(req, ct);

        if (response.Bunch.Any())
            await this.SendPartialContentAsync(response, ct);
        else
            await SendNoContentAsync(ct);
    }
}