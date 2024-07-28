using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.DataAccess.Contracts;
using VyazmaTech.Prachka.Application.Mapping;
using VyazmaTech.Prachka.Domain.Core.ValueObjects;
using static VyazmaTech.Prachka.Application.Contracts.Core.Queues.Commands.IncreaseQueueCapacity;

namespace VyazmaTech.Prachka.Application.Handlers.Core.Queue.Commands.IncreaseQueueCapacity;

internal sealed class IncreaseQueueCapacityCommandHandler : ICommandHandler<
    Command,
    Response>
{
    private readonly IPersistenceContext _persistenceContext;

    public IncreaseQueueCapacityCommandHandler(IPersistenceContext persistenceContext)
    {
        _persistenceContext = persistenceContext;
    }

    public async ValueTask<Response> Handle(Command request, CancellationToken cancellationToken)
    {
        Domain.Core.Queues.Queue queue = await _persistenceContext.Queues
            .GetByIdAsync(request.QueueId, cancellationToken);

        var capacity = Capacity.Create(request.Capacity);

        queue.IncreaseCapacity(capacity);

        await _persistenceContext.SaveChangesAsync(cancellationToken);

        return new Response(queue.ToDto());
    }
}