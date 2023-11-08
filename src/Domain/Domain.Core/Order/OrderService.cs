using Domain.Common.Errors;
using Domain.Common.Exceptions;
using Domain.Common.Result;
using Domain.Core.Queue;

namespace Domain.Core.Order;

public class OrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IQueueRepository _queueRepository;

    public OrderService(IOrderRepository orderRepository, IQueueRepository queueRepository)
    {
        _orderRepository = orderRepository;
        _queueRepository = queueRepository;
    }

    public async Task<Result<OrderEntity>> ProlongOrder(
        OrderEntity order,
        QueueEntity queue,
        DateTime prolongedOnUtc,
        CancellationToken cancellationToken)
    {
        if (order.Queue.Id.Equals(queue.Id))
        {
            var exception = new DomainException(DomainErrors.Order.UnableToTransferInTheSameQueue);
            return new Result<OrderEntity>(exception);
        }

        if (queue.Capacity.Value.Equals(queue.Items.Count))
        {
            var exception = new DomainException(DomainErrors.Queue.Overfull);
            return new Result<OrderEntity>(exception);
        }

        Result<OrderEntity> removalResult = queue.Remove(order);
        if (removalResult.IsFaulted)
        {
            return removalResult;
        }

        order.Prolong(queue, prolongedOnUtc);
        await _orderRepository.UpdateAsync(order, cancellationToken);
        await _queueRepository.UpdateAsync(queue, cancellationToken);

        return order;
    }
}