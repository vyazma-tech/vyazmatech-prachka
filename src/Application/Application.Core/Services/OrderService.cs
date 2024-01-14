using Domain.Common.Abstractions;
using Domain.Common.Errors;
using Domain.Common.Result;
using Domain.Core.Order;
using Domain.Core.Queue;
using Domain.Kernel;
using Infrastructure.DataAccess.Contracts;

namespace Application.Core.Services;

public class OrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IQueueRepository _queueRepository;
    private readonly IDateTimeProvider _timeProvider;

    public OrderService(
        IOrderRepository orderRepository,
        IQueueRepository queueRepository,
        IDateTimeProvider timeProvider)
    {
        _orderRepository = orderRepository;
        _queueRepository = queueRepository;
        _timeProvider = timeProvider;
    }

    public Result<OrderEntity> ProlongOrder(
        OrderEntity order,
        QueueEntity queue)
    {
        if (order.Queue.Id.Equals(queue.Id))
        {
            return new Result<OrderEntity>(DomainErrors.Order.UnableToTransferIntoSameQueue);
        }

        if (queue.Capacity.Value.Equals(queue.Items.Count))
        {
            return new Result<OrderEntity>(DomainErrors.Order.UnableToTransferIntoFullQueue);
        }

        Result<OrderEntity> removalResult = order.Queue.Remove(order);
        if (removalResult.IsFaulted)
        {
            return removalResult;
        }

        Result<OrderEntity> entranceResult = queue.Add(order, new SpbDateTime(_timeProvider.UtcNow));
        if (entranceResult.IsFaulted)
        {
            return entranceResult;
        }

        order.Prolong(queue, new SpbDateTime(_timeProvider.UtcNow));
        _orderRepository.Update(order);
        _queueRepository.Update(queue);

        return order;
    }
}