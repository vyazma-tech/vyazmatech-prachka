using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Domain.Common.Result;

namespace VyazmaTech.Prachka.Application.Contracts.Queues.Commands;

public static class BulkInsertOrders
{
    public record struct Command(Guid QueueId, int Quantity) : ICommand<Result<Response>>;

    public record struct Response;
}