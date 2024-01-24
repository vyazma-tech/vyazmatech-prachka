using Domain.Common.Result;
using FastEndpoints;
using Mediator;
using Presentation.Endpoints.Extensions;
using static Application.Core.Contracts.Queues.Queries.QueueById;

namespace Presentation.Endpoints.Queue.FindQueue;

public class FindQueueByIdEndpoint : Endpoint<Query, Response>
{
    private readonly ISender _sender;

    public FindQueueByIdEndpoint(ISender sender)
    {
        _sender = sender;
    }

    public override void Configure()
    {
        Get("api/queues/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Query req, CancellationToken ct)
    {
        Result<Response> response = await _sender.Send(req, ct);

        await response.Match(
            _ => SendOkAsync(response.Value, ct),
            _ => this.SendProblemsAsync(response.ToProblemDetails()));
    }
}