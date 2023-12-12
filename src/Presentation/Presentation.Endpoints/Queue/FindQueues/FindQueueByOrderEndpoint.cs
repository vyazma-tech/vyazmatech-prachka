using Application.Handlers.Queue.Queries;
using Application.Handlers.Queue.Queries.FindByOrderQueue;
using Domain.Common.Result;
using FastEndpoints;
using Mediator;
using Microsoft.AspNetCore.Http;

namespace Presentation.Endpoints.Queue.FindQueues;

internal class FindQueueByOrderEndpoint : Endpoint<FindQueueByOrderQuery, QueueResponse>
{
    private readonly IMediator _mediator;

    public FindQueueByOrderEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Verbs(Http.GET);
        Routes("api/queue/orderId/{orderId}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(FindQueueByOrderQuery orderQuery, CancellationToken ct)
    {
        Result<QueueResponse> response = await _mediator.Send(orderQuery, ct);

        if (response.IsSuccess)
        {
            await SendOkAsync(response.Value, ct);
        }
        else
        {
            AddError(response.Error.Message);
            await SendNotFoundAsync(ct);
        }
    }
}