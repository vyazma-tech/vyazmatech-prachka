using FastEndpoints;
using Mediator;
using VyazmaTech.Prachka.Application.Contracts.Queues.Commands;
using VyazmaTech.Prachka.Application.Dto.Queue;
using VyazmaTech.Prachka.Domain.Common.Result;
using VyazmaTech.Prachka.Presentation.Endpoints.Extensions;
using VyazmaTech.Prachka.Presentation.Endpoints.Queue.Models;

namespace VyazmaTech.Prachka.Presentation.Endpoints.Queue;

internal class IncreaseQueueCapacityEndpoint : Endpoint<IncreaseCapacityRequest, QueueDto>
{
    private readonly ISender _sender;

    public IncreaseQueueCapacityEndpoint(ISender sender)
    {
        _sender = sender;
    }

    public override void Configure()
    {
        Patch("api/queues/capacity/");
        AllowAnonymous();
    }

    public override async Task HandleAsync(IncreaseCapacityRequest req, CancellationToken ct)
    {
        var command = new IncreaseQueueCapacity.Command(req.QueueId, req.Capacity);

        Result<IncreaseQueueCapacity.Response> response = await _sender.Send(command, ct);

        await response.Match(
            success => SendOkAsync(success.Queue, ct),
            _ => this.SendProblemsAsync(response.ToProblemDetails()));
    }
}