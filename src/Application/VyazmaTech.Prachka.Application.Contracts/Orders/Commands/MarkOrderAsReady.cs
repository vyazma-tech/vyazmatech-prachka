using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.Dto.Order;
using VyazmaTech.Prachka.Domain.Common.Result;

namespace VyazmaTech.Prachka.Application.Contracts.Orders.Commands;

public static class MarkOrderAsReady
{
    public record struct Command(Guid Id) : IValidatableRequest<Result<Response>>;

    public record struct Response(OrderDto Order);
}