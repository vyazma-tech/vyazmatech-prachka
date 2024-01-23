namespace Application.DataAccess.Contracts.Querying.Queue;

public interface IQueryBuilder
{
    IQueryBuilder WithId(Guid id);

    IQueryBuilder WithAssignmentDate(DateOnly assignmentDate);

    IQueryBuilder WithOrderId(Guid orderId);

    IQueryBuilder WithPage(int page);

    IQueryBuilder WithLimit(int limit);

    QueueQuery Build();
}