using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.Dto.Queue;
using VyazmaTech.Prachka.Domain.Common.Result;

namespace VyazmaTech.Prachka.Application.Contracts.Queues.Commands;

public static class ChangeQueueActivityBoundaries
{
    public record struct Command(Guid QueueId, TimeOnly ActiveFrom, TimeOnly ActiveUntil) : ICommand<Result<Response>>;

    public record struct Response(QueueDto Queue);
}