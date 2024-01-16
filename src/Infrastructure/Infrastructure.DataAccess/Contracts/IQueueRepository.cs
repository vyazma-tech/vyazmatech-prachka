using Domain.Common.Result;
using Domain.Core.Queue;
using Infrastructure.DataAccess.Models;

namespace Infrastructure.DataAccess.Contracts;

public interface IQueueRepository
{
    Task<Result<QueueEntity>> FindByAsync(
        Specification<QueueModel> specification,
        CancellationToken cancellationToken);

    IAsyncEnumerable<QueueEntity> QueryAsync(
        Specification<QueueModel> specification,
        CancellationToken cancellationToken);

    void InsertRange(IReadOnlyCollection<QueueEntity> queues);

    void Update(QueueEntity queue);

    Task<long> CountAsync(Specification<QueueModel> specification, CancellationToken cancellationToken);
}