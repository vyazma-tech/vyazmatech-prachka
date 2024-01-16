using Domain.Common.Result;
using FastEndpoints;
using Mediator;
using Presentation.Endpoints.Extensions;
using static Application.Handlers.Order.Queries.OrderById.OrderByIdQuery;

namespace Presentation.Endpoints.Order.FindOrders;

internal class FindOrderByIdEndpoint : Endpoint<Query, Response>
{
    private readonly ISender _sender;

    public FindOrderByIdEndpoint(ISender sender)
    {
        _sender = sender;
    }

    public override void Configure()
    {
        Get("api/orders/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Query req, CancellationToken ct)
    {
        Result<Response> response = await _sender.Send(req, ct);

        await response.Match(
            success => SendOkAsync(success, ct),
            _ => this.SendProblemsAsync(response.ToProblemDetails()));
    }
}