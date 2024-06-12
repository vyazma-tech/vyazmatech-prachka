using VyazmaTech.Prachka.Application.Contracts.Common;

namespace VyazmaTech.Prachka.Application.Contracts.Subscriptions;

public static class SubscribeToQueue
{
    public record struct Command(Guid QueueId, Guid? UserId) : ICommand<Response>;

    public record struct Response;
}