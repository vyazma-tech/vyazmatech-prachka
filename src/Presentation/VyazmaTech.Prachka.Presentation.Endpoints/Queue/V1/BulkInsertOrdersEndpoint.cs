using FastEndpoints;
using Mediator;
using VyazmaTech.Prachka.Application.Contracts.Core.Queues.Commands;
using VyazmaTech.Prachka.Domain.Common.Exceptions;
using VyazmaTech.Prachka.Presentation.Endpoints.Extensions;
using VyazmaTech.Prachka.Presentation.Endpoints.Queue.V1.Models;

namespace VyazmaTech.Prachka.Presentation.Endpoints.Queue.V1;

internal sealed class BulkInsertOrdersEndpoint : Endpoint<BulkInsertOrdersRequest>
{
    private readonly ISender _sender;

    public BulkInsertOrdersEndpoint(ISender sender)
    {
        _sender = sender;
    }

    public override void Configure()
    {
        Post("{queueId:guid}/entrance");
        Group<QueueEndpointGroup>();
        Version(1);
    }

    public override async Task HandleAsync(BulkInsertOrdersRequest req, CancellationToken ct)
    {
        var command = new BulkInsertOrders.Command(req.QueueId, req.Quantity);

        try
        {
            BulkInsertOrders.Response response = await _sender.Send(command, ct);
            await SendOkAsync(ct);
        }
        catch (DomainException e)
        {
            await this.SendProblemsAsync(e.ToProblemDetails());
        }
    }
}