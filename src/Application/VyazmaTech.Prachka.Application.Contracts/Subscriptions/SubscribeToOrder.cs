using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Domain.Common.Result;

namespace VyazmaTech.Prachka.Application.Contracts.Subscriptions;

public static class SubscribeToOrder
{
    public record struct Command(Guid OrderId, Guid? UserId) : ICommand<Result<Response>>;

    public record struct Response;
}