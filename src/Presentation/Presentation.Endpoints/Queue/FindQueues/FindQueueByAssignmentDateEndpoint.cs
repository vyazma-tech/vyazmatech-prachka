using Application.Handlers.Queue.Queries;
using Application.Handlers.Queue.Queries.FindByAssignmentDateQueue;
using Domain.Common.Result;
using FastEndpoints;
using Mediator;
using Microsoft.AspNetCore.Http;

namespace Presentation.Endpoints.Queue.FindQueues;

internal class FindQueueByAssignmentDateEndpoint : Endpoint<FindQueueByAssignmentDateQuery, Result<QueueResponse>>
{
    private readonly IMediator _mediator;

    public FindQueueByAssignmentDateEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Verbs(Http.GET);
        Routes("api/queue/assignmentDate/{AssignmentDate}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(FindQueueByAssignmentDateQuery dateQuery, CancellationToken ct)
    {
        Result<QueueResponse> response = await _mediator.Send(dateQuery, ct);

        if (response.IsSuccess)
        {
            await SendOkAsync(response, ct);
        }
        else
        {
            AddError(response.Error.Message);
            await SendNotFoundAsync(ct);
        }
    }
}