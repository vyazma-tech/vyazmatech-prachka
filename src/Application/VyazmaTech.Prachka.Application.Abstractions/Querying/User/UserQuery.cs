namespace VyazmaTech.Prachka.Application.Abstractions.Querying.User;

public sealed class UserQuery
{
    private UserQuery(Guid? id, string? fullname, DateOnly? registrationDate, string? telegramId, int? page, int? limit)
    {
        Id = id;
        Fullname = fullname;
        RegistrationDate = registrationDate;
        TelegramId = telegramId;
        Page = page;
        Limit = limit;
    }

    public static UserQuery Build(Action<IQueryBuilder> expression)
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

    public string? Fullname { get; }

    public DateOnly? RegistrationDate { get; }

    public string? TelegramId { get; }

    public int? Page { get; }

    public int? Limit { get; }

    internal class QueryBuilder : IQueryBuilder
    {
        private Guid? _id;
        private string? _fullname;
        private DateOnly? _registrationDate;
        private string? _telegramId;
        private int? _page;
        private int? _limit;

        public IQueryBuilder WithId(Guid id)
        {
            _id = id;
            return this;
        }

        public IQueryBuilder WithFullname(string fullname)
        {
            _fullname = fullname;
            return this;
        }

        public IQueryBuilder WithRegistrationDate(DateOnly registrationDate)
        {
            _registrationDate = registrationDate;
            return this;
        }

        public IQueryBuilder WithTelegramId(string telegramId)
        {
            _telegramId = telegramId;
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

        public UserQuery Build()
        {
            return new UserQuery(
                _id,
                _fullname,
                _registrationDate,
                _telegramId,
                _page,
                _limit);
        }
    }
}