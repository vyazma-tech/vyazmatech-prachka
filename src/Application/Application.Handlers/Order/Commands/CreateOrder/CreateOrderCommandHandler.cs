using Application.Core.Contracts;
using Application.Handlers.Mapping;
using Application.Handlers.Queue.Commands.CreateQueue;
using Domain.Common.Result;
using Domain.Core.Order;
using Domain.Core.Queue;
using Domain.Core.User;
using Infrastructure.DataAccess.Contracts;
using Infrastructure.DataAccess.Specifications.Queue;
using Infrastructure.DataAccess.Specifications.User;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.Order.Commands.CreateOrder;

internal sealed class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, CreateOrderResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IQueueRepository _queueRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger<CreateOrderCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public CreateOrderCommandHandler(
        IUserRepository userRepository,
        IQueueRepository queueRepository,
        IOrderRepository orderRepository,
        ILogger<CreateOrderCommandHandler> logger,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _queueRepository = queueRepository;
        _orderRepository = orderRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async ValueTask<CreateOrderResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var ordersToCreate = new List<OrderEntity>();
        foreach (CreateOrderModel order in request.Orders)
        {
            var orderIdSpec = new UserByIdSpecification(order.UserId);
            Result<UserEntity> userByIdResult = await _userRepository
                .FindByAsync(orderIdSpec, cancellationToken);

            if (userByIdResult.IsFaulted)
            {
                _logger.LogWarning(
                    "Order {Order} cannot be created due to: {Error}",
                    order,
                    userByIdResult.Error.Message);

                continue;
            }
        
            var queueIdSpec = new QueueByIdSpecification(order.QueueId);
            Result<QueueEntity> queueByIdResult = await _queueRepository
                .FindByAsync(queueIdSpec, cancellationToken);
            
            if (queueByIdResult.IsFaulted)
            {
                _logger.LogWarning(
                    "Order {Order} cannot be created due to: {Error}",
                    order,
                    queueByIdResult.Error.Message);

                continue;
            }
            
            DateTime dateUtc = order.CreationDate.ToUniversalTime();
            Result<OrderEntity> newOrderResult = OrderEntity.Create(userByIdResult.Value, queueByIdResult.Value, dateUtc);

            if (newOrderResult.IsFaulted)
            {
                _logger.LogWarning(
                    "Order {Order} cannot be created due to: {Error}",
                    order,
                    newOrderResult.Error.Message);

                continue;
            }
            
            ordersToCreate.Add(newOrderResult.Value);
        }
        await _orderRepository.InsertRangeAsync(ordersToCreate, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        var response = new CreateOrderResponse(ordersToCreate
            .Select(x => new CreateOrderResponseModel
            {
                Id = x.Id,
                CreationDateUtc = x.CreationDate,
                ModifiedOn = x.ModifiedOn,
                Paid = x.Paid,
                Ready = x.Ready,
                Queue = x.Queue.ToDto(),
            }).ToList());

        return response;
    }
}