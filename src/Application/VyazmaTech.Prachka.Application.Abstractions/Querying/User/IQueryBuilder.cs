namespace VyazmaTech.Prachka.Application.Abstractions.Querying.User;

public interface IQueryBuilder
{
    IQueryBuilder WithId(Guid id);

    IQueryBuilder WithFullname(string fullname);

    IQueryBuilder WithRegistrationDate(DateOnly registrationDate);

    IQueryBuilder WithTelegramId(string telegramId);

    IQueryBuilder WithPage(int page);

    IQueryBuilder WithLimit(int limit);

    UserQuery Build();
}