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

internal sealed class CreateOrderCommandHandler : ICommandHandler<CreateOrdersCommand, CreateOrdersResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IRepository<QueueEntity> _queueRepository;
    private readonly IRepository<OrderEntity> _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateOrderCommandHandler(
        IUserRepository userRepository,
        // IQueueRepository queueRepository,
        IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _queueRepository = _unitOfWork.GetRepository<QueueEntity>();
        _orderRepository = _unitOfWork.GetRepository<OrderEntity>();
    }

    public async ValueTask<CreateOrdersResponse> Handle(CreateOrdersCommand request, CancellationToken cancellationToken)
    {
        var ordersToCreate = new List<OrderEntity>();
        foreach (CreateOrderModel order in request.Orders)
        {
            var userIdSpec = new UserByIdSpecification(order.UserId);
            Result<UserEntity> userByIdResult = await _userRepository
                .FindByAsync(userIdSpec, cancellationToken);
        
            var queueIdSpec = new QueueByIdSpecification(order.QueueId);
            queueIdSpec.AddInclude(queue => queue.Items);
            queueIdSpec.AsNoTracking = true;
            
            Result<QueueEntity> queueByIdResult = await _queueRepository
                .FindByAsync(queueIdSpec, cancellationToken);
            
            // UTC?
            DateOnly dateUtc = order.CreationDate;
            Result<OrderEntity> newOrderResult = OrderEntity.Create(
                userByIdResult.Value,
                queueByIdResult.Value,
                dateUtc);
            
            ordersToCreate.Add(newOrderResult.Value);
        }
        await _orderRepository.InsertRangeAsync(ordersToCreate, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        var response = new CreateOrdersResponse(ordersToCreate
            .Select(x => x.ToCreationDto()).ToList());

        return response;
    }
}