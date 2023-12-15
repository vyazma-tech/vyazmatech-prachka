using Application.Core.Contracts;
using Domain.Common.Result;
using Domain.Core.Queue;
using Domain.Core.ValueObjects;
using Domain.Kernel;
using Infrastructure.DataAccess.Contracts;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.Queue.Commands.CreateQueue;

internal sealed class CreateQueuesCommandHandler : ICommandHandler<CreateQueuesCommand, CreateQueuesResponse>
{
    private readonly ILogger<CreateQueuesCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<QueueEntity> _queueRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public CreateQueuesCommandHandler(
        ILogger<CreateQueuesCommandHandler> logger,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider)
    {
        _logger = logger;
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

            if (capacityCreationResult.IsFaulted)
            {
                _logger.LogWarning(
                    "Queue {Queue} cannot be created due to: {Error}",
                    queue,
                    capacityCreationResult.Error.Message);

                continue;
            }

            Result<QueueActivityBoundaries> activityBoundariesCreationResult =
                QueueActivityBoundaries.Create(queue.ActiveFrom, queue.ActiveUntil);

            if (activityBoundariesCreationResult.IsFaulted)
            {
                _logger.LogWarning(
                    "Queue {Queue} cannot be created due to: {Error}",
                    queue,
                    activityBoundariesCreationResult.Error.Message);

                continue;
            }

            Result<QueueDate> assignmentDateCreationResult =
                QueueDate.Create(queue.AssignmentDate, _dateTimeProvider);

            if (assignmentDateCreationResult.IsFaulted)
            {
                _logger.LogWarning(
                    "Queue {Queue} cannot be created due to: {Error}",
                    queue,
                    assignmentDateCreationResult.Error.Message);
                
                continue;
            }

            queuesToCreate.Add(new QueueEntity(
                capacityCreationResult.Value,
                assignmentDateCreationResult.Value,
                activityBoundariesCreationResult.Value));
        }

        await _queueRepository.InsertRangeAsync(queuesToCreate, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var response = new CreateQueuesResponse(queuesToCreate
            .Select(x => new QueueResponseModel
            {
                Id = x.Id,
                Capacity = x.Capacity.Value,
                AssignmentDate = x.CreationDate,
                ActiveFrom = x.ActivityBoundaries.ActiveFrom,
                ActiveUntil = x.ActivityBoundaries.ActiveUntil
            }).ToList());

        return response;
    }
}