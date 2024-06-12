using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.Core.Specifications;
using VyazmaTech.Prachka.Application.DataAccess.Contracts;
using VyazmaTech.Prachka.Application.Mapping;
using VyazmaTech.Prachka.Domain.Core.Queue;
using VyazmaTech.Prachka.Domain.Core.ValueObjects;
using VyazmaTech.Prachka.Domain.Kernel;
using static VyazmaTech.Prachka.Application.Contracts.Queues.Commands.IncreaseQueueCapacity;

namespace VyazmaTech.Prachka.Application.Handlers.Queue.Commands.IncreaseQueueCapacity;

internal sealed class IncreaseQueueCapacityCommandHandler : ICommandHandler<Command, Response>
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

    public async ValueTask<Response> Handle(Command request, CancellationToken cancellationToken)
    {
        QueueEntity queue = await _persistenceContext.Queues
            .FindByIdAsync(request.QueueId, cancellationToken);

        Capacity capacity = Capacity.Create(request.Capacity);

        queue.IncreaseCapacity(capacity, _dateTimeProvider.UtcNow);

        _persistenceContext.Queues.Update(queue);
        await _persistenceContext.SaveChangesAsync(cancellationToken);

        return new Response(queue.ToDto());
    }
}