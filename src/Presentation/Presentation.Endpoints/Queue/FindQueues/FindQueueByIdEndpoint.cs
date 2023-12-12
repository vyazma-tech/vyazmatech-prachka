using Application.Handlers.Queue.Queries;
using Application.Handlers.Queue.Queries.FindByIdQueue;
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
        Routes("api/queue/queueId/{QueueId}");
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
            AddError(response.Error.Message);
            await SendNotFoundAsync(ct);
        }
    }
}