using FastEndpoints;
using Mediator;
using VyazmaTech.Prachka.Application.Contracts.Queues.Queries;
using VyazmaTech.Prachka.Application.Dto.Queue;
using VyazmaTech.Prachka.Domain.Common.Result;
using VyazmaTech.Prachka.Presentation.Endpoints.Extensions;
using VyazmaTech.Prachka.Presentation.Endpoints.Queue.V1.Models;

namespace VyazmaTech.Prachka.Presentation.Endpoints.Queue.V1;

internal class FindQueueByIdEndpoint : Endpoint<QueueWithIdRequest, QueueDto>
{
    private readonly ISender _sender;

    public FindQueueByIdEndpoint(ISender sender)
    {
        _sender = sender;
    }

    public override void Configure()
    {
        Get("{id}");
        AllowAnonymous();
        Group<QueueEndpointGroup>();
        Version(1);
    }

    public override async Task HandleAsync(QueueWithIdRequest req, CancellationToken ct)
    {
        var query = new QueueById.Query(req.QueueId);

        Result<QueueById.Response> response = await _sender.Send(query, ct);

        await response.Match(
            _ => SendOkAsync(response.Value.Queue, ct),
            _ => this.SendProblemsAsync(response.ToProblemDetails()));
    }
}