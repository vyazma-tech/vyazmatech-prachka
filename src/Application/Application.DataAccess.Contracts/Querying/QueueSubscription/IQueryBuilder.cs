namespace Application.DataAccess.Contracts.Querying.QueueSubscription;

public interface IQueryBuilder
{
    IQueryBuilder WithId(Guid id);

    IQueryBuilder WithUserId(Guid userId);

    IQueryBuilder WithQueueId(Guid queueId);

    IQueryBuilder WithPage(int page);

    IQueryBuilder WithLimit(int limit);

    QueueSubscriptionQuery Build();
}