using VyazmaTech.Prachka.Application.Abstractions.Querying.Queue;
using VyazmaTech.Prachka.Application.DataAccess.Contracts.Repositories;
using VyazmaTech.Prachka.Domain.Common.Errors;
using VyazmaTech.Prachka.Domain.Common.Result;
using VyazmaTech.Prachka.Domain.Core.Queue;

namespace VyazmaTech.Prachka.Application.Core.Specifications;

public static class QueueSpecifications
{
    public static async Task<Result<QueueEntity>> FindByIdAsync(
        this IQueueRepository repository,
        Guid id,
        CancellationToken token)
    {
        var query = QueueQuery.Build(x => x.WithId(id));

        QueueEntity? result = await repository.QueryAsync(query, token).SingleOrDefaultAsync(token);

        if (result is null)
        {
            return new Result<QueueEntity>(DomainErrors.Entity.NotFoundFor<QueueEntity>(id.ToString()));
        }

        return result;
    }

    public static async Task<Result<QueueEntity>> FindByAssignmentDateAsync(
        this IQueueRepository repository,
        DateOnly assignmentDate,
        CancellationToken token)
    {
        var query = QueueQuery.Build(x => x.WithAssignmentDate(assignmentDate));

        QueueEntity? result = await repository.QueryAsync(query, token).SingleOrDefaultAsync(token);

        if (result is null)
        {
            return new Result<QueueEntity>(DomainErrors.Entity.NotFoundFor<QueueEntity>(assignmentDate.ToString()));
        }

        return result;
    }
}