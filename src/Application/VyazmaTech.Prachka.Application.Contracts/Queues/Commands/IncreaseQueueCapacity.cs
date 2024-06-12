using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.Dto.Queue;

namespace VyazmaTech.Prachka.Application.Contracts.Queues.Commands;

public static class IncreaseQueueCapacity
{
    public record struct Command(Guid QueueId, int Capacity) : ICommand<Response>;

    public record struct Response(QueueDto Queue);
}