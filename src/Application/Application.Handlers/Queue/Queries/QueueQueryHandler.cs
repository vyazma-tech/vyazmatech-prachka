using System.Collections.ObjectModel;
using Application.Core.Common;
using Application.Core.Configuration;
using Application.Core.Contracts;
using Application.Handlers.Mapping;
using Application.Handlers.Mapping.QueueMapping;
using Application.Handlers.Order.Queries;
using Domain.Common.Result;
using Domain.Core.Order;
using Domain.Core.Queue;
using Domain.Core.User;
using Infrastructure.DataAccess.Quering.Abstractions;
using Infrastructure.DataAccess.Specifications.Order;
using Infrastructure.DataAccess.Specifications.Queue;
using Infrastructure.DataAccess.Specifications.User;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Application.Handlers.Queue.Queries;

internal sealed class QueueQueryHandler : IQueryHandler<QueueQuery, QueueResponse>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IQueueRepository _queueRepository;
    private readonly ILogger<QueueQueryHandler> _logger;
    
    private readonly IModelFilter<QueueEntity, QueueQueryParameter> _filter;

    public QueueQueryHandler(
        IOrderRepository orderRepository,
        IQueueRepository queueRepository,
        IModelFilter<QueueEntity, QueueQueryParameter> filter,
        ILogger<QueueQueryHandler> logger)
    {
        _orderRepository = orderRepository;
        _queueRepository = queueRepository;
        _filter = filter;
        _logger = logger;
    }

    public async ValueTask<QueueResponse> Handle(QueueQuery request, CancellationToken cancellationToken)
    {
        var queues = new List<QueueEntity>();
        if (request.AssignmentDate.HasValue)
        {
            DateOnly assignmentDate = request.AssignmentDate.Value;
            var assignmentDateSpec = new QueueByAssignmentDateSpecification(assignmentDate);
            Result<QueueEntity> queueByDate = await _queueRepository
                .FindByAsync(assignmentDateSpec, cancellationToken);
            if (queueByDate.IsSuccess)
            {
                queues.Add(queueByDate.Value);
            }
            else
            {
                _logger.LogWarning(
                    "Queue {Queue} cannot be found due to: {Error}",
                    queueByDate.Value,
                    queueByDate.Error.Message);
            }
        }
        
        if (request.QueueId.HasValue)
        {
            var idSpec = new QueueByIdSpecification(request.QueueId.Value);
            Result<QueueEntity> queueById = await _queueRepository
                .FindByAsync(idSpec, cancellationToken);
            if (queueById.IsSuccess)
            {
                if (!queues.Any())
                {
                    queues.Add(queueById.Value);
                }
                else if (!queues.Contains(queueById.Value))
                {
                    _logger.LogWarning(
                        "Queue {Queue} cannot be found due to: {Error}",
                        queueById.Value,
                        queueById.Error.Message);
                }
            }
            else
            {
                _logger.LogWarning(
                    "Queue {Queue} cannot be found due to: {Error}",
                    queueById.Value,
                    queueById.Error.Message);
            }
        }
        
        
        if (request.OrderId.HasValue)
        {
            var orderIdSpec = new OrderByIdSpecification(request.OrderId.Value);
            
            Result<OrderEntity> orderResult = await _orderRepository
                .FindByAsync(orderIdSpec, cancellationToken);

            if (orderResult.IsSuccess)
            {
                var userSpec = new QueueByOrderSpecification(orderResult.Value);
                Result<QueueEntity> orderByUser = await _queueRepository
                    .FindByAsync(userSpec, cancellationToken);
                if (orderByUser.IsSuccess)
                {
                    if (!queues.Any())
                    {
                        queues.Add(orderByUser.Value);
                    }
                    else if (!queues.Contains(orderByUser.Value))
                    {
                        _logger.LogWarning(
                            "Queue {Queue} cannot be found due to: {Error}",
                            orderByUser.Value,
                            orderByUser.Error.Message);
                    }
                }
                else
                {
                    _logger.LogWarning(
                        "Queue {Queue} cannot be found due to: {Error}",
                        orderByUser.Value,
                        orderByUser.Error.Message);
                }
            }
        }

        QueueResponseModel queueResponseModel = queues.First().ToDto();
        return new QueueResponse(queueResponseModel);
    }
}