namespace VyazmaTech.Prachka.Application.Abstractions.Querying.QueueSubscription;

public sealed class QueueSubscriptionQuery
{
    private QueueSubscriptionQuery(Guid? id, Guid? userId, Guid? queueId, int? page, int? limit)
    {
        Id = id;
        UserId = userId;
        QueueId = queueId;
        Page = page;
        Limit = limit;
    }

    public static QueueSubscriptionQuery Build(Action<IQueryBuilder> expression)
    {
        IQueryBuilder builder = Builder();
        expression.Invoke(builder);

        return builder.Build();
    }

    public static IQueryBuilder Builder() => new QueryBuilder();

    public Guid? Id { get; }

    public Guid? UserId { get; }

    public Guid? QueueId { get; }

    public int? Page { get; }

    public int? Limit { get; }

    internal class QueryBuilder : IQueryBuilder
    {
        private Guid? _id;
        private Guid? _userId;
        private Guid? _queueId;
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

        public IQueryBuilder WithQueueId(Guid queueId)
        {
            _queueId = queueId;
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

        public QueueSubscriptionQuery Build()
        {
            return new QueueSubscriptionQuery(
                id: _id,
                userId: _userId,
                queueId: _queueId,
                page: _page,
                limit: _limit);
        }
    }
}