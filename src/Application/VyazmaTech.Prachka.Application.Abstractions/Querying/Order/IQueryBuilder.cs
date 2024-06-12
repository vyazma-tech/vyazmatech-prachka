namespace VyazmaTech.Prachka.Application.Abstractions.Querying.Order;

public interface IQueryBuilder
{
    IQueryBuilder WithId(Guid id);

    IQueryBuilder WithUserId(Guid userId);

    IQueryBuilder WithQueueId(Guid queueId);

    IQueryBuilder WithStatus(string status);

    IQueryBuilder WithCreationDate(DateTime creationDate);

    IQueryBuilder WithPage(int page);

    IQueryBuilder WithLimit(int limit);

    OrderQuery Build();
}