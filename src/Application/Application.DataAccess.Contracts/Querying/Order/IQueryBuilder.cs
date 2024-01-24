using Domain.Common.Abstractions;

namespace Application.DataAccess.Contracts.Querying.Order;

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