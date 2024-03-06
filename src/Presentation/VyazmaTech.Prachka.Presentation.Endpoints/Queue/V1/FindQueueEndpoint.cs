using FastEndpoints;
using Mediator;
using VyazmaTech.Prachka.Application.Contracts.Queues.Queries;
using VyazmaTech.Prachka.Application.Dto;
using VyazmaTech.Prachka.Application.Dto.Queue;
using VyazmaTech.Prachka.Presentation.Endpoints.Extensions;
using VyazmaTech.Prachka.Presentation.Endpoints.Queue.V1.Models;

namespace VyazmaTech.Prachka.Presentation.Endpoints.Queue.V1;

internal class FindQueueEndpoint : Endpoint<FindQueuesRequest, PagedResponse<QueueDto>>
{
    private readonly ISender _sender;

    public FindQueueEndpoint(ISender sender)
    {
        _sender = sender;
    }

    public override void Configure()
    {
        Get("/");
        AllowAnonymous();
        Group<QueueEndpointGroup>();
        Version(1);
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