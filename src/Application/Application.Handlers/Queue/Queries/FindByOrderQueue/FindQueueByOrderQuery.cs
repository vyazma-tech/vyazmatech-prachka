using Application.Core.Contracts;
using Domain.Common.Result;

namespace Application.Handlers.Queue.Queries.FindByOrderQueue;

public sealed class FindQueueByOrderQuery : IQuery<Result<QueueResponse>>
{
    public FindQueueByOrderQuery(Guid orderId)
    {
        OrderId = orderId;
    }

    public Guid OrderId { get; set; }
}