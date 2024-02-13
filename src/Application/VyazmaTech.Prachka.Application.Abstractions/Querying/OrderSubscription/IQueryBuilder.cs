namespace VyazmaTech.Prachka.Application.Abstractions.Querying.OrderSubscription;

public interface IQueryBuilder
{
    IQueryBuilder WithId(Guid id);

    IQueryBuilder WithUserId(Guid userId);

    IQueryBuilder WithOrderId(Guid orderId);

    IQueryBuilder WithPage(int page);

    IQueryBuilder WithLimit(int limit);

    OrderSubscriptionQuery Build();
}