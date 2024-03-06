using VyazmaTech.Prachka.Application.Dto;
using VyazmaTech.Prachka.Application.Dto.Order;
using VyazmaTech.Prachka.Domain.Core.Order;

namespace VyazmaTech.Prachka.Application.Mapping;

public static class OrderMapping
{
    public static OrderDto ToDto(this OrderEntity order)
    {
        return new OrderDto(
            order.Id,
            order.User.Id,
            order.User.Telegram,
            order.User.Fullname,
            order.Queue,
            order.Status.ToString(),
            order.CreationDate,
            order.ModifiedOn?.Value);
    }

    public static OrderInfoDto ToDto(this OrderInfo orderInfo)
    {
        return new OrderInfoDto(
            OrderId: orderInfo.Id,
            UserId: orderInfo.User.Id,
            TelegramUsername: orderInfo.User.Telegram,
            Fullname: orderInfo.User.Fullname,
            Status: orderInfo.Status.ToString());
    }

    public static PagedResponse<OrderDto> ToPagedResponse(
        this IEnumerable<OrderDto> orders,
        int currentPage,
        long totalPages,
        int recordsPerPage)
    {
        return new PagedResponse<OrderDto>
        {
            Bunch = orders.ToArray(),
            CurrentPage = currentPage,
            TotalPages = totalPages,
            RecordPerPage = recordsPerPage
        };
    }
}