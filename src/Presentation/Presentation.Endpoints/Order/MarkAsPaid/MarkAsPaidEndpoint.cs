using Application.Handlers.Order.Commands.MarkOrderAsPaid;
using FastEndpoints;
using Mediator;

namespace Presentation.Endpoints.Order.MarkAsPaid;

public class MarkAsPaidEndpoint : Endpoint<MarkOrderAsPaidCommand, Task>
{
    private readonly IMediator _mediator;

    public MarkAsPaidEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Verbs(Http.POST);
        Routes("api/order/mark-as-paid");
        AllowAnonymous();
    }

    public override async Task HandleAsync(MarkOrderAsPaidCommand req, CancellationToken ct)
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