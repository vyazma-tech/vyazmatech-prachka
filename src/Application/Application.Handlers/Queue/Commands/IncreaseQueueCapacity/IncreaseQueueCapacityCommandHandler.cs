using Application.Core.Contracts;
using Domain.Common.Result;
using Domain.Core.Queue;
using Domain.Core.ValueObjects;
using Domain.Kernel;
using Infrastructure.DataAccess.Contracts;
using Infrastructure.DataAccess.Specifications.Queue;
using static Application.Handlers.Queue.Commands.IncreaseQueueCapacity.IncreaseQueueCapacity;

namespace Application.Handlers.Queue.Commands.IncreaseQueueCapacity;

internal sealed class IncreaseQueueCapacityCommandHandler : ICommandHandler<Command, Result<Response>>
{
    private readonly IPersistenceContext _persistenceContext;
    private readonly IDateTimeProvider _dateTimeProvider;

    public IncreaseQueueCapacityCommandHandler(
        IDateTimeProvider dateTimeProvider,
        IPersistenceContext persistenceContext)
    {
        _persistenceContext = persistenceContext;
        _dateTimeProvider = dateTimeProvider;
    }

    public async ValueTask<Result<Response>> Handle(Command request, CancellationToken cancellationToken)
    {
        Result<QueueEntity> searchResult = await _persistenceContext.Queues
            .FindByAsync(
                new QueueByIdSpecification(request.QueueId),
                cancellationToken);

        Result<Capacity> capacityValidationResult = Capacity.Create(request.Capacity);

        if (searchResult.IsFaulted)
        {
            return new Result<Response>(searchResult.Error);
        }

        if (capacityValidationResult.IsFaulted)
        {
            return new Result<Response>(searchResult.Error);
        }

        QueueEntity queue = searchResult.Value;
        Capacity newCapacity = capacityValidationResult.Value;
        Result<QueueEntity> increaseResult = queue
            .IncreaseCapacity(
                newCapacity,
                _dateTimeProvider.SpbDateTimeNow);

        if (increaseResult.IsFaulted)
        {
            return new Result<Response>(increaseResult.Error);
        }

        queue = increaseResult.Value;

        _persistenceContext.Queues.Update(queue);
        await _persistenceContext.SaveChangesAsync(cancellationToken);

        return queue.ToDto();
    }
}