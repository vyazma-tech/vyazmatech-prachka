using Application.Core.Common;
using FastEndpoints;
using Mediator;
using Presentation.Endpoints.Extensions;
using static Application.Core.Contracts.Queues.Queries.QueueByQuery;

namespace Presentation.Endpoints.Queue.FindQueue;

public class FindQueueEndpoint : Endpoint<Query, PagedResponse<Response>>
{
    private readonly ISender _sender;

    public FindQueueEndpoint(ISender sender)
    {
        _sender = sender;
    }

    public override void Configure()
    {
        Get("api/queues");
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