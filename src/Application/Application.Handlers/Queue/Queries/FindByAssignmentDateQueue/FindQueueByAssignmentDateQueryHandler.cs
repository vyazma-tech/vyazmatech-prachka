using Application.Core.Contracts;
using Application.Handlers.Mapping;
using Domain.Common.Result;
using Domain.Core.Queue;
using Infrastructure.DataAccess.Specifications.Queue;

namespace Application.Handlers.Queue.Queries.FindByAssignmentDateQueue;

internal sealed class FindQueueByAssignmentDateQueryHandler : IQueryHandler<FindQueueByAssignmentDateQuery, Result<QueueResponse>>
{
    private readonly IQueueRepository _queueRepository;

    public FindQueueByAssignmentDateQueryHandler(IQueueRepository queueRepository)
    {
        _queueRepository = queueRepository;
    }

    public async ValueTask<Result<QueueResponse>> Handle(FindQueueByAssignmentDateQuery request, CancellationToken cancellationToken)
    {
        DateTime utcDate = request.AssignmentDate.ToUniversalTime();
        
        var queueByPredicateSpecification = new QueueByAssignmentDateSpecification(
            utcDate);

        Result<QueueEntity> queueEntityResult = await _queueRepository
            .FindByAsync(queueByPredicateSpecification, cancellationToken);
        
        if (queueEntityResult.IsSuccess)
        {
            return new QueueResponse(queueEntityResult.Value.ToDto());
        }

        return new Result<QueueResponse>(queueEntityResult.Error);
    }
}