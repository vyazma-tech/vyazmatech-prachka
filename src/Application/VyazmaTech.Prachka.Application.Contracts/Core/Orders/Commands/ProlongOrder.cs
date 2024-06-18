using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.Dto.Core.Order;

namespace VyazmaTech.Prachka.Application.Contracts.Core.Orders.Commands;

public static class ProlongOrder
{
    public record Command(Guid OrderId, Guid TargetQueueId) : IValidatableRequest<Response>;

    public record struct Response(OrderDto Order);
}