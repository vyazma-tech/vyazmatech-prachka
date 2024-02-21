using FastEndpoints;
using Mediator;
using VyazmaTech.Prachka.Application.Contracts.Queues.Queries;
using VyazmaTech.Prachka.Application.Dto;
using VyazmaTech.Prachka.Application.Dto.Queue;
using VyazmaTech.Prachka.Presentation.Endpoints.Extensions;
using VyazmaTech.Prachka.Presentation.Endpoints.Queue.Models;

namespace VyazmaTech.Prachka.Presentation.Endpoints.Queue;

internal class FindQueueEndpoint : Endpoint<FindQueuesRequest, PagedResponse<QueueDto>>
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

    public override async Task HandleAsync(FindQueuesRequest req, CancellationToken ct)
    {
        var query = new QueueByQuery.Query(req.AssignmentDate, req.OrderId, req.Page);

        QueueByQuery.Response response = await _sender.Send(query, ct);

        if (response.Queues.Bunch.Any())
            await this.SendPartialContentAsync(response, ct);
        else
            await SendNoContentAsync(ct);
    }
}