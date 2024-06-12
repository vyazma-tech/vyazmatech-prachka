using FastEndpoints;
using Mediator;
using VyazmaTech.Prachka.Application.Contracts.Queues.Commands;
using VyazmaTech.Prachka.Application.Dto.Queue;
using VyazmaTech.Prachka.Domain.Common.Exceptions;
using VyazmaTech.Prachka.Presentation.Authorization;
using VyazmaTech.Prachka.Presentation.Endpoints.Extensions;
using VyazmaTech.Prachka.Presentation.Endpoints.Queue.V1.Models;

namespace VyazmaTech.Prachka.Presentation.Endpoints.Queue.V1;

internal class IncreaseQueueCapacityEndpoint : Endpoint<IncreaseCapacityRequest, QueueDto>
{
    private const string FeatureName = "IncreaseCapacity";
    private readonly ISender _sender;

    public IncreaseQueueCapacityEndpoint(ISender sender)
    {
        _sender = sender;
    }

    public override void Configure()
    {
        Patch("capacity");
        Policies($"{AuthorizeFeatureAttribute.Prefix}{FeatureScope.Name}:{FeatureName}");
        Group<QueueEndpointGroup>();
        Version(1);
    }

    public override async Task HandleAsync(IncreaseCapacityRequest req, CancellationToken ct)
    {
        var command = new IncreaseQueueCapacity.Command(req.QueueId, req.Capacity);

        try
        {
            IncreaseQueueCapacity.Response response = await _sender.Send(command, ct);
            await SendOkAsync(response.Queue, ct);
        }
        catch (DomainException e)
        {
            await this.SendProblemsAsync(e.ToProblemDetails());
        }
    }
}