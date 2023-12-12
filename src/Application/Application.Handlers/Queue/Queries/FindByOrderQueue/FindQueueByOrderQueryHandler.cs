using Application.Core.Contracts;
using Application.Handlers.Mapping;
using Domain.Common.Result;
using Domain.Core.Order;
using Domain.Core.Queue;
using Infrastructure.DataAccess.Specifications.Order;
using Infrastructure.DataAccess.Specifications.Queue;

namespace Application.Handlers.Queue.Queries.FindByOrderQueue;

internal sealed class FindQueueByOrderQueryHandler : IQueryHandler<FindQueueByOrderQuery, Result<QueueResponse>>
{
    private readonly IQueueRepository _queueRepository;
    private readonly IOrderRepository _orderRepository;

    public FindQueueByOrderQueryHandler(IQueueRepository queueRepository, IOrderRepository orderRepository)
    {
        _queueRepository = queueRepository;
        _orderRepository = orderRepository;
    }

    public async ValueTask<Result<QueueResponse>> Handle(FindQueueByOrderQuery request, CancellationToken cancellationToken)
    {
        var orderSpec = new OrderByIdSpecification(request.OrderId);
        Result<OrderEntity> orderResult = await _orderRepository.FindByAsync(orderSpec, cancellationToken);
        
        QueueByOrderSpecification queueByPredicateSpecification;
        if (orderResult.IsSuccess)
        {
            queueByPredicateSpecification = new QueueByOrderSpecification(orderResult.Value);
        }
        else
        {
            return new Result<QueueResponse>(orderResult.Error);
        }
        
        Result<QueueEntity> queueEntityResult = await _queueRepository
            .FindByAsync(queueByPredicateSpecification, cancellationToken);
        
        if (queueEntityResult.IsSuccess)
        {
            return new QueueResponse(queueEntityResult.Value.ToDto());
        }

        return new Result<QueueResponse>(queueEntityResult.Error);
    }
}