using Application.Handlers.Queue.Commands.CreateQueue;
using Application.Handlers.Queue.Queries;
using Domain.Common.Result;
using FastEndpoints;
using Mediator;
using Microsoft.AspNetCore.Http;

namespace Presentation.Endpoints.Queue.FindQueues;

internal class FindQueueByIdEndpoint : Endpoint<FindQueueByIdQuery, QueueResponse>
{
    private readonly IMediator _mediator;

    public FindQueueByIdEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Verbs(Http.GET);
        Routes("api/queue/{QueueId}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(FindQueueByIdQuery query, CancellationToken ct)
    {
        Result<QueueResponse> response = await _mediator.Send(query, ct);

        if (response.IsSuccess)
        {
            await SendOkAsync(response.Value, ct);
        }
        else
        {
            await SendAsync(response.Value, StatusCodes.Status400BadRequest, ct);
        }
    }
}