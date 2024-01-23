using Application.DataAccess.Contracts.Querying.Queue;
using Application.DataAccess.Contracts.Repositories;
using Domain.Common.Errors;
using Domain.Common.Result;
using Domain.Core.Queue;

namespace Application.Core.Specifications;

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