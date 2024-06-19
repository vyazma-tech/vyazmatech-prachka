using FastEndpoints;
using Mediator;
using VyazmaTech.Prachka.Application.Contracts.Core.Queues.Queries;
using VyazmaTech.Prachka.Application.Dto.Core.Queue;
using VyazmaTech.Prachka.Domain.Common.Exceptions;
using VyazmaTech.Prachka.Presentation.Endpoints.Extensions;
using VyazmaTech.Prachka.Presentation.Endpoints.Queue.V1.Models;

namespace VyazmaTech.Prachka.Presentation.Endpoints.Queue.V1;

internal class FindQueueByIdEndpoint : Endpoint<QueueWithIdRequest, QueueWithOrdersDto>
{
    private readonly ISender _sender;

    public FindQueueByIdEndpoint(ISender sender)
    {
        _sender = sender;
    }

    public override void Configure()
    {
        Get("{queueId}");
        AllowAnonymous();
        Group<QueueEndpointGroup>();
        Version(1);
    }

    public override async Task HandleAsync(QueueWithIdRequest req, CancellationToken ct)
    {
        var query = new QueueById.Query(req.QueueId);

        try
        {
            QueueById.Response response = await _sender.Send(query, ct);
            await SendOkAsync(response.Queue, ct);
        }
        catch (DomainException e)
        {
            await this.SendProblemsAsync(e.ToProblemDetails());
        }
    }
}