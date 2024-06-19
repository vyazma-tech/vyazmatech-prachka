using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.Dto.Core.Queue;

namespace VyazmaTech.Prachka.Application.Contracts.Core.Queues.Commands;

public static class ChangeQueueActivityBoundaries
{
    public record struct Command(Guid QueueId, TimeOnly ActiveFrom, TimeOnly ActiveUntil) : ICommand<Response>;

    public record struct Response(QueueDto Queue);
}