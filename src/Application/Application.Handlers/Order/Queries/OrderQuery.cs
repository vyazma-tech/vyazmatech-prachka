using Application.Core.Common;
using Application.Core.Contracts;

namespace Application.Handlers.Order.Queries;

public class OrderQuery : IQuery<PagedResponse<OrderResponse>>
{
    public OrderQuery(Guid? orderId, Guid? userId, DateOnly? creationDate, int? page)
    {
        OrderId = orderId;
        UserId = userId;
        CreationDate = creationDate;
        Page = page;
    }
    
    public static QueryBuilder Builder => new QueryBuilder();
    public Guid? OrderId { get; set; }
    public Guid? UserId { get; set; }
    public DateOnly? CreationDate { get; set; }
    public int? Page { get; set; }

    public sealed class QueryBuilder
    {
        private Guid? _orderId;
        private Guid? _userId;
        private DateOnly? _creationDate;
        private int? _page;

        public QueryBuilder WithId(Guid id)
        {
            _orderId = id;
            return this;
        }

        public QueryBuilder WithUserId(Guid userId)
        {
            _userId = userId;
            return this;
        }

        public QueryBuilder WithCreationDate(DateOnly creationDate)
        {
            _creationDate = creationDate;
            return this;
        }

        public QueryBuilder WithPage(int page)
        {
            _page = page;
            return this;
        }

        public OrderQuery Build()
        {
            return new OrderQuery(_orderId, _userId, _creationDate, _page);
        }
    }
}