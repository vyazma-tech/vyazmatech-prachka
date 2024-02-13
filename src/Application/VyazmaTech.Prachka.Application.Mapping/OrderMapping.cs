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
            order.User,
            order.Queue,
            order.Status.ToString(),
            order.CreationDate,
            order.ModifiedOn?.Value);
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