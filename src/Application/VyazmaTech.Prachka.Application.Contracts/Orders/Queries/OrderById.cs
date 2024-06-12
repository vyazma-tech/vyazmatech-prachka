using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.Dto.Order;

namespace VyazmaTech.Prachka.Application.Contracts.Orders.Queries;

public static class OrderById
{
    public record struct Query(Guid Id) : IQuery<Response>;

    public record struct Response(OrderDto Order);
}