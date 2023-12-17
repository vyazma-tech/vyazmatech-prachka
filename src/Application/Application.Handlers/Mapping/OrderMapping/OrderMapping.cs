using Application.Handlers.Order.Commands.CreateOrder;
using Application.Handlers.Order.Queries;
using Domain.Core.Order;

namespace Application.Handlers.Mapping.OrderMapping;

public static class OrderMapping
{
    public static OrderResponseModel ToDto(this OrderEntity orderEntity)
    {
        return new OrderResponseModel
        {
            Id = orderEntity.Id,
            Paid = orderEntity.Paid,
            Ready = orderEntity.Ready,
            ModifiedOn = orderEntity.ModifiedOn,
            CreationDate = orderEntity.CreationDate,
        };
    }

    public static CreateOrdersResponseModel ToCreationDto(this OrderEntity orderEntity)
    {
        return new CreateOrdersResponseModel
        {
            Id = orderEntity.Id,
            Paid = orderEntity.Paid,
            Ready = orderEntity.Ready,
            ModifiedOn = orderEntity.ModifiedOn,
            CreationDate = orderEntity.CreationDate,
        };
    } 
}