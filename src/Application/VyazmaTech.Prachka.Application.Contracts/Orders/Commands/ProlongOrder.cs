using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.Dto.Order;
using VyazmaTech.Prachka.Domain.Common.Result;

namespace VyazmaTech.Prachka.Application.Contracts.Orders.Commands;

public static class ProlongOrder
{
    public record Command(Guid OrderId, Guid TargetQueueId) : IValidatableRequest<Result<Response>>;

    public record struct Response(OrderDto Order);
}