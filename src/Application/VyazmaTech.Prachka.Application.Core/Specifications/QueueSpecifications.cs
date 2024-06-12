using VyazmaTech.Prachka.Application.Abstractions.Querying.Queue;
using VyazmaTech.Prachka.Application.DataAccess.Contracts.Repositories;
using VyazmaTech.Prachka.Domain.Common.Errors;
using VyazmaTech.Prachka.Domain.Common.Exceptions;
using VyazmaTech.Prachka.Domain.Core.Queues;

namespace VyazmaTech.Prachka.Application.Core.Specifications;

public static class QueueSpecifications
{
    public static async Task<Queue> FindByIdAsync(
        this IQueueRepository repository,
        Guid id,
        CancellationToken token)
    {
        var query = QueueQuery.Build(x => x.WithId(id));

        Queue? result = await repository.QueryAsync(query, token).SingleOrDefaultAsync(token);

        if (result is null)
            throw new NotFoundException(DomainErrors.Entity.NotFoundFor<Queue>(id.ToString()));

        return result;
    }

    public static async Task<Queue> FindByAssignmentDateAsync(
        this IQueueRepository repository,
        DateOnly assignmentDate,
        CancellationToken token)
    {
        var query = QueueQuery.Build(x => x.WithAssignmentDate(assignmentDate));

        Queue? result = await repository.QueryAsync(query, token).SingleOrDefaultAsync(token);

        if (result is null)
            throw new NotFoundException(DomainErrors.Entity.NotFoundFor<Queue>(assignmentDate.ToString()));

        return result;
    }
}