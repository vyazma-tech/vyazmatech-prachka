using Application.Core.Contracts;
using Application.Handlers.Mapping.QueueMapping;
using Application.Handlers.Queue.Queries;
using Domain.Common.Errors;
using Domain.Common.Result;
using Domain.Core.Queue;
using Domain.Core.ValueObjects;
using Domain.Kernel;
using Infrastructure.DataAccess.Contracts;
using Infrastructure.DataAccess.Specifications.Queue;

namespace Application.Handlers.Queue.Commands.ChangeQueueActivityBoundaries;

public class ChangeQueueActivityBoundariesHandler
    : ICommandHandler<ChangeQueueActivityBoundariesCommand, Result<QueueResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<QueueEntity> _queueRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public ChangeQueueActivityBoundariesHandler(
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider)
    {
        _unitOfWork = unitOfWork;
        _queueRepository = unitOfWork.GetRepository<QueueEntity>();
        _dateTimeProvider = dateTimeProvider;
    }
    
    public async ValueTask<Result<QueueResponse>> Handle(
        ChangeQueueActivityBoundariesCommand request,
        CancellationToken cancellationToken)
    {
        var queueByIdSpecification = new QueueByIdSpecification(request.QueueId);
        Result<QueueEntity> queueEntityResult = await _queueRepository
            .FindByAsync(queueByIdSpecification, cancellationToken);

        Result<QueueActivityBoundaries> newActivityBoundaries = 
            QueueActivityBoundaries.Create(request.ActiveFrom, request.ActiveUntil);
        Result<QueueEntity> changedActiveTimeQueueResult =
            queueEntityResult.Value.ChangeActivityBoundaries(newActivityBoundaries.Value, _dateTimeProvider.UtcNow);

        if (changedActiveTimeQueueResult.IsFaulted)
            return new Result<QueueResponse>(changedActiveTimeQueueResult.Error);
        
        _queueRepository.Update(queueEntityResult.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new QueueResponse(queueEntityResult.Value.ToDto());
    }
}