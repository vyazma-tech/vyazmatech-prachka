using FastEndpoints;
using Mediator;
using VyazmaTech.Prachka.Application.Contracts.Queues.Commands;
using VyazmaTech.Prachka.Domain.Common.Result;
using VyazmaTech.Prachka.Presentation.Endpoints.Extensions;
using VyazmaTech.Prachka.Presentation.Endpoints.Queue.V1.Models;

namespace VyazmaTech.Prachka.Presentation.Endpoints.Queue.V1;

internal sealed class BulkRemoveOrdersEndpoint : Endpoint<BulkInsertOrdersRequest>
{
    private readonly ISender _sender;

    public BulkRemoveOrdersEndpoint(ISender sender)
    {
        _sender = sender;
    }

    public override void Configure()
    {
        Post("{queueId:guid}/exit");
        Group<QueueEndpointGroup>();
        Version(1);
    }

    public override async Task HandleAsync(BulkInsertOrdersRequest req, CancellationToken ct)
    {
        var command = new BulkRemoveOrders.Command(req.QueueId, req.Quantity);

        Result<BulkRemoveOrders.Response> response = await _sender.Send(command, ct);

        await response.Match(
            _ => SendOkAsync(cancellation: ct),
            _ => this.SendProblemsAsync(response.ToProblemDetails()));
    }
}