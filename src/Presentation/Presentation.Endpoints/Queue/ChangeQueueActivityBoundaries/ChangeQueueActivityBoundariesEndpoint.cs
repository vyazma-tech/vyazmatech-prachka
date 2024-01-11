using System.Net;
using Application.Handlers.Queue.Commands.ChangeQueueActivityBoundaries;
using Application.Handlers.Queue.Queries;
using Domain.Common.Result;
using FastEndpoints;
using Mediator;
using Microsoft.AspNetCore.Http;

namespace Presentation.Endpoints.Queue.ChangeQueueActivityBoundaries;

public class ChangeQueueActivityBoundariesEndpoint : Endpoint<ChangeQueueActivityBoundariesCommand>
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
            await SendOkAsync(response.Value, ct);
        }
        else
        {
            // AddError(response.Error.Message);
            await SendAsync(response.Error, StatusCodes.Status400BadRequest, cancellation: ct);
        }
    }
}