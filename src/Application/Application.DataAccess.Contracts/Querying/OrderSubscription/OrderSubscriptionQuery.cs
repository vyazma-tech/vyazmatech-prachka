namespace Application.DataAccess.Contracts.Querying.OrderSubscription;

public sealed class OrderSubscriptionQuery
{
    private OrderSubscriptionQuery(Guid? id, Guid? userId, Guid? orderId, int? page, int? limit)
    {
        Id = id;
        UserId = userId;
        OrderId = orderId;
        Page = page;
        Limit = limit;
    }

    public static OrderSubscriptionQuery Build(Action<IQueryBuilder> expression)
    {
        IQueryBuilder builder = Builder();
        expression.Invoke(builder);

        return builder.Build();
    }

    public static IQueryBuilder Builder() => new QueryBuilder();

    public Guid? Id { get; }

    public Guid? UserId { get; }

    public Guid? OrderId { get; }

    public int? Page { get; }

    public int? Limit { get; }

    internal class QueryBuilder : IQueryBuilder
    {
        private Guid? _id;
        private Guid? _userId;
        private Guid? _orderId;
        private int? _page;
        private int? _limit;

        public IQueryBuilder WithId(Guid id)
        {
            _id = id;
            return this;
        }

        public IQueryBuilder WithUserId(Guid userId)
        {
            _userId = userId;
            return this;
        }

        public IQueryBuilder WithOrderId(Guid orderId)
        {
            _orderId = orderId;
            return this;
        }

        public IQueryBuilder WithPage(int page)
        {
            _page = page;
            return this;
        }

        public IQueryBuilder WithLimit(int limit)
        {
            _limit = limit;
            return this;
        }

        public OrderSubscriptionQuery Build()
        {
            return new OrderSubscriptionQuery(
                id: _id,
                userId: _userId,
                orderId: _orderId,
                page: _page,
                limit: _limit);
        }
    }
}