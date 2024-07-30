using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.Dto.Core.Order;

namespace VyazmaTech.Prachka.Application.Contracts.Core.Orders.Queries;

public static class MyOrders
{
    public readonly record struct Query : IQuery<Response>;

    public readonly record struct Response(IReadOnlyCollection<MyOrdersDto> History);
}