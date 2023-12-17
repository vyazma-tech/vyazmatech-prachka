using System.Net;
using Application.Handlers.Queue.Commands.ChangeQueueActivityBoundaries;
using Application.Handlers.Queue.Queries;
using Domain.Common.Result;
using FastEndpoints;
using Mediator;
using Microsoft.AspNetCore.Http;

namespace Presentation.Endpoints.Queue.ChangeQueueActivityBoundaries;

public class ChangeQueueActivityBoundariesEndpoint : Endpoint<ChangeQueueActivityBoundariesCommand, Result<QueueResponse>>
{
    private readonly IMediator _mediator;

    public ChangeQueueActivityBoundariesEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Verbs(Http.PATCH);
        Routes("api/queue/change-activity-boundaries");
        AllowAnonymous();
    }

    public override async Task HandleAsync(ChangeQueueActivityBoundariesCommand req, CancellationToken ct)
    {
        Result<QueueResponse> response = await _mediator.Send(req, ct);

        if (response.IsSuccess)
        {
            await SendOkAsync(response, ct);
        }
        else
        {
            await SendAsync(response, StatusCodes.Status400BadRequest, ct);
        }
    }
}