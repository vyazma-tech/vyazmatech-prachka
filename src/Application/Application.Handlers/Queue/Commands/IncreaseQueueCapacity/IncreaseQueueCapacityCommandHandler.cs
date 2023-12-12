using Application.Core.Contracts;
using Domain.Common.Result;
using Domain.Core.Queue;
using Domain.Core.ValueObjects;
using Domain.Kernel;
using Infrastructure.DataAccess.Contracts;
using Infrastructure.DataAccess.Specifications.Queue;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.Queue.Commands.IncreaseQueueCapacity;

public class IncreaseQueueCapacityCommandHandler : ICommandHandler<IncreaseQueueCapacityCommand, Task>
{
    private readonly ILogger<IncreaseQueueCapacityCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<QueueEntity> _queueRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public IncreaseQueueCapacityCommandHandler(
        ILogger<IncreaseQueueCapacityCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _queueRepository = unitOfWork.GetRepository<QueueEntity>();
        _dateTimeProvider = dateTimeProvider;
    }

    public async ValueTask<Task> Handle(IncreaseQueueCapacityCommand request, CancellationToken cancellationToken)
    {
        var queueByIdSpecification = new QueueByIdSpecification(request.QueueId);
        Result<QueueEntity> queueEntityResult = await _queueRepository
            .FindByAsync(queueByIdSpecification, cancellationToken);
        
        if (queueEntityResult.IsFaulted)
        {
            _logger.LogWarning(
                "Queue {Queue} cannot be found due to: {Error}",
                queueEntityResult.Value,
                queueEntityResult.Error.Message);
        }

        Result<Capacity> newCapacityResult = Capacity.Create(request.Capacity);
        if (newCapacityResult.IsFaulted)
        {
            _logger.LogWarning(
                "Capacity {Capacity} cannot be created due to: {Error}",
                newCapacityResult.Value,
                newCapacityResult.Error.Message);
        }
        
        Result<QueueEntity> increasedQueueCapacityResult = queueEntityResult
            .Value
            .IncreaseCapacity(newCapacityResult.Value, _dateTimeProvider.UtcNow);
        
        if (increasedQueueCapacityResult.IsFaulted)
        {
            _logger.LogWarning(
                "Capacity of queue {Queue} cannot be increased due to: {Error}",
                increasedQueueCapacityResult.Value,
                increasedQueueCapacityResult.Error.Message);
        }
        
        _queueRepository.Update(increasedQueueCapacityResult.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Task.CompletedTask;
    }
}