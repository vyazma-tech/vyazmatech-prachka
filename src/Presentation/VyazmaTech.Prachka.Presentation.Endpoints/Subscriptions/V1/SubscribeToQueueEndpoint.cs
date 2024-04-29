using FastEndpoints;
using Mediator;
using VyazmaTech.Prachka.Application.Abstractions.Identity;
using VyazmaTech.Prachka.Application.Contracts.Subscriptions;
using VyazmaTech.Prachka.Domain.Common.Result;
using VyazmaTech.Prachka.Presentation.Endpoints.Extensions;
using VyazmaTech.Prachka.Presentation.Endpoints.Subscriptions.V1.Models;

namespace VyazmaTech.Prachka.Presentation.Endpoints.Subscriptions.V1;

internal sealed class SubscribeToQueueEndpoint : Endpoint<SubscribeToQueueRequest>
{
    private readonly ISender _sender;
    private readonly ICurrentUser _currentUser;

    public SubscribeToQueueEndpoint(ISender sender, ICurrentUser currentUser)
    {
        _sender = sender;
        _currentUser = currentUser;
    }

    public override void Configure()
    {
        Post("queues/{queueId:guid}");
        Group<SubscriptionEndpointGroup>();
        Version(1);
    }

    public override async Task HandleAsync(SubscribeToQueueRequest req, CancellationToken ct)
    {
        Guid? userId = _currentUser.Id;

        var command = new SubscribeToQueue.Command(req.QueueId, userId);

        Result<SubscribeToQueue.Response> response = await _sender.Send(command, ct);

        await response.Match(
            _ => SendOkAsync(cancellation: ct),
            _ => this.SendProblemsAsync(response.ToProblemDetails()));
    }
}