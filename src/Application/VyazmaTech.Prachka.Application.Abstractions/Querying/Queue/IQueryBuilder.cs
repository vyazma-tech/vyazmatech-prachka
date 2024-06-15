namespace VyazmaTech.Prachka.Application.Abstractions.Querying.Queue;

public interface IQueryBuilder
{
    IQueryBuilder WithSearchFromDate(DateOnly assignmentDate);

    IQueryBuilder WithPage(int page);

    IQueryBuilder WithLimit(int limit);

    QueueQuery Build();
}