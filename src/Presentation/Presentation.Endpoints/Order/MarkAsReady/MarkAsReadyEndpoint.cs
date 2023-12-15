using Application.Handlers.Order.Commands.MarkOrderAsReady;
using FastEndpoints;
using Mediator;

namespace Presentation.Endpoints.Order.MarkAsReady;

public class MarkAsReadyEndpoint : Endpoint<MarkOrderAsReadyCommand, Task>
{
    private readonly IMediator _mediator;

    public MarkAsReadyEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Verbs(Http.POST);
        Routes("api/order/mark-as-ready");
        AllowAnonymous();
    }

    public override async Task HandleAsync(MarkOrderAsReadyCommand req, CancellationToken ct)
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