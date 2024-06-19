using VyazmaTech.Prachka.Application.Contracts.Common;

namespace VyazmaTech.Prachka.Application.Contracts.Core.Queues.Commands;

public static class BulkInsertOrders
{
    public record struct Command(Guid QueueId, int Quantity) : ICommand<Response>;

    public record struct Response;
}