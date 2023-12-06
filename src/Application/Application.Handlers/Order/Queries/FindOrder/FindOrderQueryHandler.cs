using System.Collections.ObjectModel;
using Application.Core.Common;
using Application.Core.Configuration;
using Application.Core.Contracts;
using Application.Handlers.Mapping;
using Domain.Common.Result;
using Domain.Core.Order;
using Domain.Core.User;
using Infrastructure.DataAccess.Specifications.Order;
using Infrastructure.DataAccess.Specifications.User;
using Microsoft.Extensions.Options;

namespace Application.Handlers.Order.Queries.FindOrder;

internal sealed class FindOrderQueryHandler : IQueryHandler<FindOrderQuery, PagedResponse<OrderResponse>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUserRepository _userRepository;
    private readonly PaginationConfiguration _paginationConfiguration;

    public FindOrderQueryHandler(
        IOrderRepository orderRepository,
        IUserRepository userRepository,
        IOptionsMonitor<PaginationConfiguration> paginationConfiguration)
    {
        _orderRepository = orderRepository;
        _userRepository = userRepository;
        _paginationConfiguration = paginationConfiguration.CurrentValue;
    }

    public async ValueTask<PagedResponse<OrderResponse>> Handle(FindOrderQuery request, CancellationToken cancellationToken)
    {
        long totalRecords = await _userRepository.CountAsync(cancellationToken);
        
        var orders = new List<OrderEntity>();
        if (request.CreationDate.HasValue)
        {
            DateTime creationDateUtc = request.CreationDate.Value.ToUniversalTime();
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
            orders.AddRange(ordersById.Where(o => orders.Contains(o)));
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
                orders.AddRange(ordersByUser.Where(o => orders.Contains(o)));
            }
        }

        var readonlyOrders = new ReadOnlyCollection<OrderResponse>(orders.Select(o => 
            new OrderResponse(o.ToDto())).ToList());

        return new PagedResponse<OrderResponse>
        {
            Bunch = readonlyOrders,
            TotalPages = totalRecords / _paginationConfiguration.RecordsPerPage,
            RecordPerPage = _paginationConfiguration.RecordsPerPage,
            CurrentPage = request.Page
        };
    }
}