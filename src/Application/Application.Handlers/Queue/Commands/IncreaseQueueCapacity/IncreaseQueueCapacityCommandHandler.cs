using Application.Core.Common;
using Application.Core.Contracts;
using Application.DataAccess.Contracts;
using Application.Handlers.Queue.Queries;
using Domain.Common.Result;
using Domain.Core.Queue;
using Domain.Core.ValueObjects;
using Domain.Kernel;
using Infrastructure.DataAccess.Specifications.Queue;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.Queue.Commands.IncreaseQueueCapacity;

// TODO: FIX IT
// internal sealed class
//     IncreaseQueueCapacityCommandHandler : ICommandHandler<IncreaseQueueCapacityCommand, ResultResponse<QueueResponse>>
// {
//     private readonly IUnitOfWork _unitOfWork;
//     private readonly IQueueRepository _queueRepository;
//     private readonly IDateTimeProvider _dateTimeProvider;
//
//     public IncreaseQueueCapacityCommandHandler(
//         IUnitOfWork unitOfWork,
//         IDateTimeProvider dateTimeProvider,
//         IPersistenceContext persistenceContext)
//     {
//         _unitOfWork = unitOfWork;
//         _queueRepository = persistenceContext.Queues;
//         _dateTimeProvider = dateTimeProvider;
//     }
//
//     // TODO: refactor this
//     public async ValueTask<ResultResponse<QueueResponse>> Handle(
//         IncreaseQueueCapacityCommand request,
//         CancellationToken cancellationToken)
//     {
//         var queueByIdSpecification = new QueueByIdSpecification(request.QueueId);
//         Result<QueueEntity> queueEntityResult = await _queueRepository
//             .FindByAsync(queueByIdSpecification, cancellationToken);
//
//         Result<Capacity> newCapacityResult = Capacity.Create(request.Capacity);
//
//         Result<QueueEntity> increasedQueueCapacityResult = queueEntityResult
//             .Value
//             .IncreaseCapacity(newCapacityResult.Value, _dateTimeProvider.UtcNow);
//
//         if (increasedQueueCapacityResult.IsFaulted)
//             return new ResultResponse<QueueResponse>(increasedQueueCapacityResult.Error);
//
//         _queueRepository.Update(increasedQueueCapacityResult.Value);
//         await _unitOfWork.SaveChangesAsync(cancellationToken);
//
//         return increasedQueueCapacityResult.Value.ToDto();
//     }
// }