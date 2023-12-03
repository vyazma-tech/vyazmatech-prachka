using Application.Core.Common;
using Application.Core.Contracts;
using Domain.Common.Result;

namespace Application.Handlers.Order.Queries.FindOrder;

public sealed class FindOrderQuery : IQuery<PagedResponse<OrderResponse>>
{
    public FindOrderQuery(Guid? orderId, Guid? userId, DateTime? creationDate)
    {
        OrderId = orderId;
        UserId = userId;
        CreationDate = creationDate;
    }
    
    public Guid? OrderId { get; set; }
    public Guid? UserId { get; set; }
    public DateTime? CreationDate  { get; set; }
    public int Page { get; set; } = 1;
}