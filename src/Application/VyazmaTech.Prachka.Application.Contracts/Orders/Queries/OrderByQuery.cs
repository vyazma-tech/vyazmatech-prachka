using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.Dto;
using VyazmaTech.Prachka.Application.Dto.Order;

namespace VyazmaTech.Prachka.Application.Contracts.Orders.Queries;

public static class OrderByQuery
{
    public record struct Query(Guid? UserId, Guid? QueueId, DateTime? CreationDate, string? Status, int? Page) : IQuery<Response>;

    public record struct Response(PagedResponse<OrderDto> Orders);
}