using VyazmaTech.Prachka.Application.Dto;
using VyazmaTech.Prachka.Application.Dto.Order;
using VyazmaTech.Prachka.Domain.Core.Orders;

namespace VyazmaTech.Prachka.Application.Mapping;

public static class OrderMapping
{
    public static OrderDto ToDto(this Order order)
    {
        return new OrderDto(
            order.Id,
            order.User.Id,
            order.User.TelegramUsername,
            order.User.Fullname,
            order.Queue.Id,
            order.Status.ToString(),
            order.CreationDate,
            order.ModifiedOnUtc);
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
            RecordPerPage = recordsPerPage,
        };
    }
}