namespace VyazmaTech.Prachka.Application.Abstractions.Querying.Queue;

public sealed class QueueQuery
{
    private QueueQuery(DateOnly searchFrom, int page, int limit)
    {
        SearchFrom = searchFrom;
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

    public DateOnly SearchFrom { get; }

    public int Page { get; }

    public int Limit { get; }

    internal class QueryBuilder : IQueryBuilder
    {
        private DateOnly _assignmentDate;
        private int _page;
        private int _limit;

        public IQueryBuilder WithSearchFromDate(DateOnly assignmentDate)
        {
            _assignmentDate = assignmentDate;
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
                _assignmentDate,
                _page,
                _limit);
        }
    }
}