using Application.Core.Contracts;
using Application.Handlers.Mapping.QueueMapping;
using Domain.Common.Result;
using Domain.Core.Queue;
using Domain.Core.ValueObjects;
using Domain.Kernel;
using Infrastructure.DataAccess.Contracts;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.Queue.Commands.CreateQueue;

internal sealed class CreateQueuesCommandHandler : ICommandHandler<CreateQueuesCommand, CreateQueuesResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<QueueEntity> _queueRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public CreateQueuesCommandHandler(
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider)
    {
        _unitOfWork = unitOfWork;
        _queueRepository = _unitOfWork.GetRepository<QueueEntity>();
        _dateTimeProvider = dateTimeProvider;
    }

    public async ValueTask<CreateQueuesResponse> Handle(
        CreateQueuesCommand request,
        CancellationToken cancellationToken)
    {
        var queuesToCreate = new List<QueueEntity>();
        foreach (QueueModel queue in request.Queues)
        {
            Result<Capacity> capacityCreationResult = Capacity.Create(queue.Capacity);

            Result<QueueActivityBoundaries> activityBoundariesCreationResult =
                QueueActivityBoundaries.Create(queue.ActiveFrom, queue.ActiveUntil);

            Result<QueueDate> assignmentDateCreationResult =
                QueueDate.Create(queue.AssignmentDate, _dateTimeProvider);

            queuesToCreate.Add(new QueueEntity(
                capacityCreationResult.Value,
                assignmentDateCreationResult.Value,
                activityBoundariesCreationResult.Value));
        }

        await _queueRepository.InsertRangeAsync(queuesToCreate, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var response = new CreateQueuesResponse(queuesToCreate
            .Select(x => x.ToCreationDto()).ToList());

        return response;
    }
}