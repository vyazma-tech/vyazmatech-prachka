namespace VyazmaTech.Prachka.Application.Abstractions.Querying.Queue;

public sealed class QueueQuery
{
    private QueueQuery(Guid? id, DateOnly? assignmentDate, Guid? orderId, int? page, int? limit)
    {
        Id = id;
        AssignmentDate = assignmentDate;
        OrderId = orderId;
        Page = page;
        Limit = limit;
    }

    public static QueueQuery Build(Action<IQueryBuilder> expression)
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

    public DateOnly? AssignmentDate { get; }

    public Guid? OrderId { get; }

    public int? Page { get; }

    public int? Limit { get; }

    internal class QueryBuilder : IQueryBuilder
    {
        private Guid? _id;
        private DateOnly? _assignmentDate;
        private Guid? _orderId;
        private int? _page;
        private int? _limit;

        public IQueryBuilder WithId(Guid id)
        {
            _id = id;
            return this;
        }

        public IQueryBuilder WithAssignmentDate(DateOnly assignmentDate)
        {
            _assignmentDate = assignmentDate;
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

        public QueueQuery Build()
        {
            return new QueueQuery(
                _id,
                _assignmentDate,
                _orderId,
                _page,
                _limit);
        }
    }
}