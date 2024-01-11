using Application.Core.Common;
using FastEndpoints;
using Mediator;
using Presentation.Endpoints.Extensions;
using static Application.Handlers.User.Queries.UserByQuery.UserByQueryQuery;

namespace Presentation.Endpoints.User.FindUsers;

internal class FindUsersEndpoint : Endpoint<Query, PagedResponse<Response>>
{
    private readonly ISender _sender;

    public FindUsersEndpoint(ISender sender)
    {
        _sender = sender;
    }

    public override void Configure()
    {
        Get("api/users");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Query req, CancellationToken ct)
    {
        PagedResponse<Response> response = await _sender.Send(req, ct);

        if (response.Bunch.Any())
            await this.SendPartialContentAsync(response, ct);
        else
            await SendNoContentAsync(ct);
    }
}