using Domain.Common.Result;
using Domain.Core.Queue;
using Infrastructure.DataAccess.Models;

namespace Infrastructure.DataAccess.Contracts;

public interface IQueueRepository
{
    Task<Result<QueueEntity>> FindByAsync(
        Specification<QueueModel> specification,
        CancellationToken cancellationToken);

    void InsertRange(IReadOnlyCollection<QueueEntity> queues, CancellationToken cancellationToken);

    void Update(QueueEntity queue);

    Task<long> CountAsync(Specification<QueueModel> specification, CancellationToken cancellationToken);
}