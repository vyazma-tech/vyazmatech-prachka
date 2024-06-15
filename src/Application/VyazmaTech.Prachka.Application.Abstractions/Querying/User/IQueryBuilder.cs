namespace VyazmaTech.Prachka.Application.Abstractions.Querying.User;

public interface IQueryBuilder
{
    IQueryBuilder WithFullname(string fullname);

    IQueryBuilder WithRegistrationDate(DateOnly registrationDate);

    IQueryBuilder WithTelegramUsername(string telegramId);

    IQueryBuilder WithPage(int page);

    IQueryBuilder WithLimit(int limit);

    UserQuery Build();
}