using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.Dto.Order;

namespace VyazmaTech.Prachka.Application.Contracts.Orders.Commands;

public static class MarkOrderAsReady
{
    public record struct Command(Guid Id) : IValidatableRequest<Response>;

    public record struct Response(OrderDto Order);
}