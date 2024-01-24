﻿using Application.Core.Contracts.Common;
using Application.Core.Specifications;
using Application.DataAccess.Contracts;
using Domain.Common.Result;
using Domain.Core.Queue;
using Domain.Core.ValueObjects;
using Domain.Kernel;
using static Application.Core.Contracts.Queues.Commands.IncreaseQueueCapacity;

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
            .FindByIdAsync(request.QueueId, cancellationToken);

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