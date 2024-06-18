using FastEndpoints;
using Mediator;
using VyazmaTech.Prachka.Application.Contracts.Core.Queues.Commands;
using VyazmaTech.Prachka.Application.Dto.Core.Queue;
using VyazmaTech.Prachka.Domain.Common.Exceptions;
using VyazmaTech.Prachka.Presentation.Authorization;
using VyazmaTech.Prachka.Presentation.Endpoints.Extensions;
using VyazmaTech.Prachka.Presentation.Endpoints.Queue.V1.Models;

namespace VyazmaTech.Prachka.Presentation.Endpoints.Queue.V1;

internal class ChangeQueueActivityBoundariesEndpoint : Endpoint<ChangeActivityBoundariesRequest, QueueDto>
{
    private const string FeatureName = "ChangeQueueActivityBoundaries";
    private readonly ISender _sender;

    public ChangeQueueActivityBoundariesEndpoint(ISender sender)
    {
        _sender = sender;
    }

    public override void Configure()
    {
        Patch("activity");
        Policies($"{AuthorizeFeatureAttribute.Prefix}{FeatureScope.Name}:{FeatureName}");
        Group<QueueEndpointGroup>();
        Version(1);
    }

    public override async Task HandleAsync(ChangeActivityBoundariesRequest req, CancellationToken ct)
    {
        var command = new ChangeQueueActivityBoundaries.Command(req.QueueId, req.ActiveFrom, req.ActiveUntil);

        try
        {
            ChangeQueueActivityBoundaries.Response response = await _sender.Send(command, ct);
            await SendOkAsync(response.Queue, ct);
        }
        catch (DomainException e)
        {
            await this.SendProblemsAsync(e.ToProblemDetails());
        }
    }
}