using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.Dto.Core.Order;

namespace VyazmaTech.Prachka.Application.Contracts.Core.Orders.Commands;

public static class MarkOrderAsPaid
{
    public record struct Command(Guid Id) : IValidatableRequest<Response>;

    public record struct Response(OrderDto Order);
}