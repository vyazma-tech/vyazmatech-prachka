using VyazmaTech.Prachka.Application.DataAccess.Contracts.Repositories;
using VyazmaTech.Prachka.Domain.Common.Errors;
using VyazmaTech.Prachka.Domain.Common.Result;
using VyazmaTech.Prachka.Domain.Core.Order;
using VyazmaTech.Prachka.Domain.Core.Queue;
using VyazmaTech.Prachka.Domain.Kernel;

namespace VyazmaTech.Prachka.Application.Core.Services;

public class OrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IDateTimeProvider _timeProvider;

    public OrderService(
        IOrderRepository orderRepository,
        IDateTimeProvider timeProvider)
    {
        _orderRepository = orderRepository;
        _timeProvider = timeProvider;
    }

    public Result<OrderEntity> ProlongOrder(
        OrderEntity order,
        QueueEntity previousQueue,
        QueueEntity targetQueue)
    {
        if (previousQueue.Id.Equals(targetQueue.Id))
        {
            return new Result<OrderEntity>(DomainErrors.Order.UnableToTransferIntoSameQueue);
        }

        if (targetQueue.Capacity.Equals(targetQueue.Orders.Count))
        {
            return new Result<OrderEntity>(DomainErrors.Order.UnableToTransferIntoFullQueue);
        }

        Result<OrderEntity> removalResult = previousQueue.Remove(order);
        if (removalResult.IsFaulted)
        {
            return removalResult;
        }

        Result<OrderEntity> entranceResult = targetQueue.Add(order, _timeProvider.UtcNow);
        if (entranceResult.IsFaulted)
        {
            return entranceResult;
        }

        order.Prolong(targetQueue, _timeProvider.UtcNow);
        _orderRepository.Update(order);

        return order;
    }
}