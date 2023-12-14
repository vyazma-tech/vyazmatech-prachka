using Application.Core.Contracts;
using Application.Handlers.Mapping.OrderMapping;
using Domain.Common.Result;
using Domain.Core.Order;
using Domain.Core.Queue;
using Domain.Core.User;
using Infrastructure.DataAccess.Contracts;
using Infrastructure.DataAccess.Specifications.Queue;
using Infrastructure.DataAccess.Specifications.User;

namespace Application.Handlers.Order.Commands.CreateOrder;

internal sealed class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, CreateOrderResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IQueueRepository _queueRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateOrderCommandHandler(
        IUserRepository userRepository,
        IQueueRepository queueRepository,
        IOrderRepository orderRepository,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _queueRepository = queueRepository;
        _orderRepository = orderRepository;
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
        
            var queueIdSpec = new QueueByIdSpecification(order.QueueId);
            Result<QueueEntity> queueByIdResult = await _queueRepository
                .FindByAsync(queueIdSpec, cancellationToken);
            
            DateTime dateUtc = order.CreationDate.ToUniversalTime();
            Result<OrderEntity> newOrderResult = OrderEntity.Create(userByIdResult.Value, queueByIdResult.Value, dateUtc);
            
            ordersToCreate.Add(newOrderResult.Value);
        }
        await _orderRepository.InsertRangeAsync(ordersToCreate, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        var response = new CreateOrderResponse(ordersToCreate
            .Select(x => x.ToCreationDto()).ToList());

        return response;
    }
}