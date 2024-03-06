using VyazmaTech.Prachka.Domain.Common.Abstractions;

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
        SpbDateTime? creationDate)
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

    public static IQueryBuilder Builder() => new QueryBuilder();

    public Guid? Id { get; }

    public Guid? UserId { get; }

    public Guid? QueueId { get; }

    public string? Status { get; }

    public int? Page { get; }

    public int? Limit { get; }

    public SpbDateTime? CreationDate { get; }

    internal class QueryBuilder : IQueryBuilder
    {
        private Guid? _id;
        private Guid? _userId;
        private Guid? _queueId;
        private string? _status;
        private int? _page;
        private int? _limit;
        private SpbDateTime? _creationDate;

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

        public IQueryBuilder WithCreationDate(SpbDateTime creationDate)
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
                id: _id,
                userId: _userId,
                queueId: _queueId,
                status: _status,
                page: _page,
                limit: _limit,
                creationDate: _creationDate);
        }
    }
}