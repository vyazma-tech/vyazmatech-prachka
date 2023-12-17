using Application.Handlers.Order.Commands.CreateOrder;
using FastEndpoints;
using Mediator;

namespace Presentation.Endpoints.Order.CreateOrders;

internal class CreateOrdersEndpoint : Endpoint<CreateOrdersCommand, CreateOrdersResponse>
{
    private readonly IMediator _mediator;

    public CreateOrdersEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Verbs(Http.POST);
        Routes("api/orders");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateOrdersCommand req, CancellationToken ct)
    {
        CreateOrdersResponse response = await _mediator.Send(req, ct);

        await SendOkAsync(response, ct);
    }
}