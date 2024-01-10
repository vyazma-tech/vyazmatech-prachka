using Application.Core.Common;
using Application.Handlers.Queue.Queries;
using Domain.Common.Result;
using FastEndpoints;
using Mediator;
using Microsoft.AspNetCore.Http;
using Presentation.Endpoints.Extensions;
using static Application.Handlers.Queue.Commands.IncreaseQueueCapacity.IncreaseQueueCapacity;

namespace Presentation.Endpoints.Queue.IncreaseQueueCapacity;

public class IncreaseQueueCapacityEndpoint : Endpoint<Command, Response>
{
    private readonly IMediator _mediator;

    public IncreaseQueueCapacityEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Verbs(Http.PATCH);
        Routes("api/queue/increase-capacity");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Command req, CancellationToken ct)
    {
        Result<Response> response = await _mediator.Send(req, ct);

        await response.Match(
            success => SendOkAsync(success, ct),
            _ => this.SendProblemsAsync(response.ToProblemDetails()));
    }
}