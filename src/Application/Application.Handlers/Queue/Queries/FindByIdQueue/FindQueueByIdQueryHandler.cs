using Application.Core.Contracts;
using Application.Handlers.Mapping;
using Domain.Common.Result;
using Domain.Core.Queue;
using Infrastructure.DataAccess.Specifications.Queue;

namespace Application.Handlers.Queue.Queries.FindByIdQueue;

public class FindQueueByIdQueryHandler : IQueryHandler<FindQueueByIdQuery, Result<QueueResponse>>
{
    private readonly IQueueRepository _queueRepository;

    public FindQueueByIdQueryHandler(IQueueRepository queueRepository)
    {
        _queueRepository = queueRepository;
    }

    public async ValueTask<Result<QueueResponse>> Handle(FindQueueByIdQuery request, CancellationToken cancellationToken)
    {
        QueueByIdSpecification queueByIdSpecification = new QueueByIdSpecification(request.QueueId);
        Result<QueueEntity> queueEntityResult = await _queueRepository
            .FindByAsync(queueByIdSpecification, cancellationToken);
        
        if (queueEntityResult.IsSuccess)
        {
            return new QueueResponse(queueEntityResult.Value.ToDto());
        }

        return new Result<QueueResponse>(queueEntityResult.Error);
    }
}