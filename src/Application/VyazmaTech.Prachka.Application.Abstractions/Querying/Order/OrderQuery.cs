namespace VyazmaTech.Prachka.Application.Abstractions.Querying.Order;

public sealed class OrderQuery
{
    private OrderQuery(
        Guid? id,
        Guid? userId,
        Guid? queueId,
        string? status,
        int? page,
        int? limit,
        DateTime? creationDate)
    {
        Id = id;
        UserId = userId;
        QueueId = queueId;
        Status = status;
        Page = page;
        Limit = limit;
        CreationDate = creationDate;
    }

    public static OrderQuery Build(Action<IQueryBuilder> expression)
    {
        IQueryBuilder builder = Builder();
        expression.Invoke(builder);

        return builder.Build();
    }

    public static IQueryBuilder Builder()
    {
        return new QueryBuilder();
    }

    public Guid? Id { get; }

    public Guid? UserId { get; }

    public Guid? QueueId { get; }

    public string? Status { get; }

    public int? Page { get; }

    public int? Limit { get; }

    public DateTime? CreationDate { get; }

    internal class QueryBuilder : IQueryBuilder
    {
        private Guid? _id;
        private Guid? _userId;
        private Guid? _queueId;
        private string? _status;
        private int? _page;
        private int? _limit;
        private DateTime? _creationDate;

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

        public IQueryBuilder WithStatus(string status)
        {
            _status = status;
            return this;
        }

        public IQueryBuilder WithCreationDate(DateTime creationDate)
        {
            _creationDate = creationDate;
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

        public OrderQuery Build()
        {
            return new OrderQuery(
                _id,
                _userId,
                _queueId,
                _status,
                _page,
                _limit,
                _creationDate);
        }
    }
}