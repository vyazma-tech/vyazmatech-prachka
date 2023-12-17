using Application.Handlers.Queue.Commands.IncreaseQueueCapacity;
using Application.Handlers.Queue.Queries;
using Domain.Common.Result;
using FastEndpoints;
using Mediator;
using Microsoft.AspNetCore.Http;

namespace Presentation.Endpoints.Queue.IncreaseQueueCapacity;

public class IncreaseQueueCapacityEndpoint : Endpoint<IncreaseQueueCapacityCommand, Result<QueueResponse>>
{
    private readonly IMediator _mediator;

    public IncreaseQueueCapacityEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Verbs(Http.PATCH);
        Routes("api/queue/increase-capacity");
        AllowAnonymous();
    }

    public override async Task HandleAsync(IncreaseQueueCapacityCommand req, CancellationToken ct)
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