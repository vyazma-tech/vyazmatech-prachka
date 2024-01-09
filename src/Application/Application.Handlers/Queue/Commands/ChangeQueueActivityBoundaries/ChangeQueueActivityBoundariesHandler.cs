using Application.Core.Contracts;
using Application.DataAccess.Contracts;
using Domain.Common.Result;
using Domain.Core.Queue;
using Domain.Core.ValueObjects;
using Domain.Kernel;
using Infrastructure.DataAccess.Contracts;
using Infrastructure.DataAccess.Specifications.Queue;
using static Application.Handlers.Queue.Commands.ChangeQueueActivityBoundaries.ChangeQueueActivityBoundaries;

namespace Application.Handlers.Queue.Commands.ChangeQueueActivityBoundaries;

internal sealed class ChangeQueueActivityBoundariesHandler
    : ICommandHandler<Command, Response>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPersistenceContext _persistenceContext;
    private readonly IDateTimeProvider _dateTimeProvider;

    public ChangeQueueActivityBoundariesHandler(
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider,
        IPersistenceContext persistenceContext)
    {
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
        _persistenceContext = persistenceContext;
    }

    public async ValueTask<Response> Handle(Command request, CancellationToken cancellationToken)
    {
        Result<QueueEntity> queueSearchResult = await _persistenceContext.Queues
            .FindByAsync(
                new QueueByIdSpecification(request.QueueId),
                cancellationToken);

        if (queueSearchResult.IsFaulted)
            throw queueSearchResult.Error;

        QueueEntity queue = queueSearchResult.Value;

        QueueActivityBoundaries newActivityBoundaries = ValidateBoundaries(
            request.ActiveFrom,
            request.ActiveUntil);

        Result<QueueEntity> changeResult = queue.ChangeActivityBoundaries(
            newActivityBoundaries,
            _dateTimeProvider.UtcNow);

        if (changeResult.IsFaulted)
            throw changeResult.Error;

        queue = changeResult.Value;
        _persistenceContext.Queues.Update(queue);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return queue.ToDto();
    }

    private static QueueActivityBoundaries ValidateBoundaries(TimeOnly from, TimeOnly until)
    {
        Result<QueueActivityBoundaries> result = QueueActivityBoundaries.Create(from, until);

        if (result.IsFaulted)
            throw result.Error;

        return result.Value;
    }
}