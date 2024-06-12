using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.Dto.Order;

namespace VyazmaTech.Prachka.Application.Contracts.Orders.Commands;

public static class ProlongOrder
{
    public record Command(Guid OrderId, Guid TargetQueueId) : IValidatableRequest<Response>;

    public record struct Response(OrderDto Order);
}