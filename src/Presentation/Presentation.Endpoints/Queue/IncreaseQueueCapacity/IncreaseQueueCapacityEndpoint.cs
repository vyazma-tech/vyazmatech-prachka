using Application.Handlers.Queue.Commands.IncreaseQueueCapacity;
using FastEndpoints;
using Mediator;

namespace Presentation.Endpoints.Queue.IncreaseQueueCapacity;

public class IncreaseQueueCapacityEndpoint : Endpoint<IncreaseQueueCapacityCommand, Task>
{
    private readonly IMediator _mediator;

    public IncreaseQueueCapacityEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Verbs(Http.POST);
        Routes("api/queue/increase-capacity");
        AllowAnonymous();
    }

    public override async Task HandleAsync(IncreaseQueueCapacityCommand req, CancellationToken ct)
    {
        Task response = await _mediator.Send(req, ct);

        try
        {
            await SendOkAsync(response, ct);
        }
        catch (Exception ex)
        {
            AddError(ex.Message);
            await SendNotFoundAsync(ct);
        }
    }
}