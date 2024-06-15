namespace VyazmaTech.Prachka.Application.Abstractions.Querying.User;

public sealed class UserQuery
{
    private UserQuery(string? fullname, DateOnly? registrationDate, string? telegramUsername, int page, int limit)
    {
        Fullname = fullname;
        RegistrationDate = registrationDate;
        TelegramUsername = telegramUsername;
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

    public string? Fullname { get; }

    public DateOnly? RegistrationDate { get; }

    public string? TelegramUsername { get; }

    public int Page { get; }

    public int Limit { get; }

    internal class QueryBuilder : IQueryBuilder
    {
        private string? _fullname;
        private DateOnly? _registrationDate;
        private string? _telegramId;
        private int _page;
        private int _limit;

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

        public IQueryBuilder WithTelegramUsername(string telegramId)
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
                _fullname,
                _registrationDate,
                _telegramId,
                _page,
                _limit);
        }
    }
}