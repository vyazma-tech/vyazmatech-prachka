using FastEndpoints;
using Mediator;
using VyazmaTech.Prachka.Application.Contracts.Orders.Queries;
using VyazmaTech.Prachka.Application.Dto;
using VyazmaTech.Prachka.Application.Dto.Order;
using VyazmaTech.Prachka.Presentation.Endpoints.Extensions;
using VyazmaTech.Prachka.Presentation.Endpoints.Order.V1.Models;

namespace VyazmaTech.Prachka.Presentation.Endpoints.Order.V1;

internal class FindOrdersEndpoint : Endpoint<FindOrdersRequest, PagedResponse<OrderDto>>
{
    private readonly ISender _sender;

    public FindOrdersEndpoint(ISender sender)
    {
        _sender = sender;
    }

    public override void Configure()
    {
        Get("/");
        AllowAnonymous();
        Group<OrderEndpointGroup>();
        Version(1);
    }

    public override async Task HandleAsync(FindOrdersRequest req, CancellationToken ct)
    {
        var query = new OrderByQuery.Query(req.UserId, req.QueueId, req.CreationDate, req.Status, req.Page);

        OrderByQuery.Response response = await _sender.Send(query, ct);

        if (response.Orders.Bunch.Any())
            await this.SendPartialContentAsync(response, ct);
        else
            await SendNoContentAsync(ct);
    }
}