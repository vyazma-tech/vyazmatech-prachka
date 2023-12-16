using System.Collections.ObjectModel;
using Application.Core.Common;
using Application.Core.Configuration;
using Application.Core.Contracts;
using Application.Handlers.Mapping;
using Application.Handlers.Mapping.OrderMapping;
using Domain.Common.Result;
using Domain.Core.Order;
using Domain.Core.User;
using Infrastructure.DataAccess.Quering.Abstractions;
using Infrastructure.DataAccess.Specifications.Order;
using Infrastructure.DataAccess.Specifications.User;
using Microsoft.Extensions.Options;

namespace Application.Handlers.Order.Queries;

internal sealed class OrderQueryHandler : IQueryHandler<OrderQuery, PagedResponse<OrderResponse>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUserRepository _userRepository;
    private readonly PaginationConfiguration _paginationConfiguration;
    
    private readonly IModelFilter<OrderEntity, OrderQueryParameter> _filter;

    public OrderQueryHandler(
        IOrderRepository orderRepository,
        IUserRepository userRepository,
        IOptionsMonitor<PaginationConfiguration> paginationConfiguration,
        IModelFilter<OrderEntity, OrderQueryParameter> filter)
    {
        _orderRepository = orderRepository;
        _userRepository = userRepository;
        _filter = filter;
        _paginationConfiguration = paginationConfiguration.CurrentValue;
    }

    public async ValueTask<PagedResponse<OrderResponse>> Handle(OrderQuery request, CancellationToken cancellationToken)
    {
        long totalRecords = await _orderRepository.CountAsync(cancellationToken);
        
        var orders = new List<OrderEntity>();
        if (request.CreationDate.HasValue)
        {
            DateOnly creationDateUtc = request.CreationDate.Value;
            var creationDateSpec = new OrderByDateSpecification(creationDateUtc);
            IReadOnlyCollection<OrderEntity> ordersByDate = await _orderRepository
                .FindAllByAsync(creationDateSpec, cancellationToken);
            orders.AddRange(ordersByDate);
        }
        
        if (request.OrderId.HasValue)
        {
            var idSpec = new OrderByIdSpecification(request.OrderId.Value);
            IReadOnlyCollection<OrderEntity> ordersById = await _orderRepository
                .FindAllByAsync(idSpec, cancellationToken);
            orders.AddRange(orders.Any() ? ordersById.Where(o => orders.Contains(o)) : ordersById);
        }
        
        
        if (request.UserId.HasValue)
        {
            var userIdSpec = new UserByIdSpecification(request.UserId.Value);
            
            Result<UserEntity> userResult = await _userRepository
                .FindByAsync(userIdSpec, cancellationToken);

            if (userResult.IsSuccess)
            {
                OrderByUserSpecification userSpec = new OrderByUserSpecification(userResult.Value);
                IReadOnlyCollection<OrderEntity> ordersByUser = await _orderRepository
                    .FindAllByAsync(userSpec, cancellationToken);
                orders.AddRange(orders.Any() ? ordersByUser.Where(o => orders.Contains(o)) : ordersByUser);
            }
        }

        var readonlyOrders = new ReadOnlyCollection<OrderResponse>(orders.Select(o => 
            new OrderResponse(o.ToDto())).ToList());

        return new PagedResponse<OrderResponse>
        {
            Bunch = readonlyOrders,
            TotalPages = totalRecords / _paginationConfiguration.RecordsPerPage,
            RecordPerPage = _paginationConfiguration.RecordsPerPage,
            CurrentPage = request.Page ?? 1
        };
    }
}