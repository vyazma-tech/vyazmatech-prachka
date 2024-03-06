using VyazmaTech.Prachka.Domain.Common.Abstractions;

namespace VyazmaTech.Prachka.Application.Abstractions.Querying.Order;

public interface IQueryBuilder
{
    IQueryBuilder WithId(Guid id);

    IQueryBuilder WithUserId(Guid userId);

    IQueryBuilder WithQueueId(Guid queueId);

    IQueryBuilder WithStatus(string status);

    IQueryBuilder WithCreationDate(SpbDateTime creationDate);

    IQueryBuilder WithPage(int page);

    IQueryBuilder WithLimit(int limit);

    OrderQuery Build();
}