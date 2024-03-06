using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.Dto.Queue;
using VyazmaTech.Prachka.Domain.Common.Result;

namespace VyazmaTech.Prachka.Application.Contracts.Queues.Commands;

public static class IncreaseQueueCapacity
{
    public record struct Command(Guid QueueId, int Capacity) : ICommand<Result<Response>>;

    public record struct Response(QueueDto Queue);
}