using VyazmaTech.Prachka.Application.Contracts.Common;

namespace VyazmaTech.Prachka.Application.Contracts.Subscriptions;

public static class SubscribeToOrder
{
    public record struct Command(Guid OrderId, Guid? UserId) : ICommand<Response>;

    public record struct Response;
}