using Application.Core.Contracts;
using Application.Handlers.Queue.Queries;
using Domain.Common.Result;

namespace Application.Handlers.Queue.Commands.ChangeQueueActivityBoundaries;

public sealed class ChangeQueueActivityBoundariesCommand : ICommand<Result<QueueResponse>>
{
    public ChangeQueueActivityBoundariesCommand(Guid queueId, TimeOnly activeFrom, TimeOnly activeUntil)
    {
        QueueId = queueId;
        ActiveFrom = activeFrom;
        ActiveUntil = activeUntil;
    }

    public Guid QueueId { get; set; }
    public TimeOnly ActiveFrom { get; set; }
    public TimeOnly ActiveUntil { get; set; }
}