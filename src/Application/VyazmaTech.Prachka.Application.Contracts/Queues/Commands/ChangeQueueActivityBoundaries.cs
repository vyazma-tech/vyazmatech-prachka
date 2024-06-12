using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.Dto.Queue;

namespace VyazmaTech.Prachka.Application.Contracts.Queues.Commands;

public static class ChangeQueueActivityBoundaries
{
    public record struct Command(Guid QueueId, TimeOnly ActiveFrom, TimeOnly ActiveUntil) : ICommand<Response>;

    public record struct Response(QueueDto Queue);
}