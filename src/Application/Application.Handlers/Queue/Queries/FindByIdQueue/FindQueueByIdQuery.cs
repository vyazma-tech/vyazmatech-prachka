using Application.Core.Contracts;
using Domain.Common.Result;

namespace Application.Handlers.Queue.Queries.FindByIdQueue;

public sealed class FindQueueByIdQuery : IQuery<Result<QueueResponse>>
{
    public FindQueueByIdQuery(Guid queueId) => QueueId = queueId;
    
    public Guid QueueId { get; set; }
}