﻿using Application.Core.Contracts.Common;
using Application.Core.Specifications;
using Application.DataAccess.Contracts;
using Domain.Common.Result;
using Domain.Core.Queue;
using Domain.Core.ValueObjects;
using Domain.Kernel;
using static Application.Core.Contracts.Queues.Commands.ChangeQueueActivityBoundaries;

namespace Application.Handlers.Queue.Commands.ChangeQueueActivityBoundaries;

internal sealed class ChangeQueueActivityBoundariesCommandHandler : ICommandHandler<Command, Result<Response>>
{
    private readonly IPersistenceContext _persistenceContext;
    private readonly IDateTimeProvider _dateTimeProvider;

    public ChangeQueueActivityBoundariesCommandHandler(
        IDateTimeProvider dateTimeProvider,
        IPersistenceContext persistenceContext)
    {
        _dateTimeProvider = dateTimeProvider;
        _persistenceContext = persistenceContext;
    }

    public async ValueTask<Result<Response>> Handle(Command request, CancellationToken cancellationToken)
    {
        Result<QueueEntity> queueSearchResult = await _persistenceContext.Queues
            .FindByIdAsync(request.QueueId, cancellationToken);

        if (queueSearchResult.IsFaulted)
        {
            return new Result<Response>(queueSearchResult.Error);
        }

        QueueEntity queue = queueSearchResult.Value;
        Result<QueueActivityBoundaries> activityBoundariesCreationResult = QueueActivityBoundaries.Create(
            request.ActiveFrom,
            request.ActiveUntil);

        if (activityBoundariesCreationResult.IsFaulted)
        {
            return new Result<Response>(activityBoundariesCreationResult.Error);
        }

        QueueActivityBoundaries newActivityBoundaries = activityBoundariesCreationResult.Value;
        Result<QueueEntity> changeResult = queue.ChangeActivityBoundaries(
            newActivityBoundaries,
            _dateTimeProvider.SpbDateTimeNow);

        if (changeResult.IsFaulted)
        {
            return new Result<Response>(changeResult.Error);
        }

        queue = changeResult.Value;
        _persistenceContext.Queues.Update(queue);
        await _persistenceContext.SaveChangesAsync(cancellationToken);

        return queue.ToDto();
    }
}