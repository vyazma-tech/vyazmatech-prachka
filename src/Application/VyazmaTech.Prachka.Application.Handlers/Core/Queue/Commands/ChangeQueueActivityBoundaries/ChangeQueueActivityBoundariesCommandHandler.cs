using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.DataAccess.Contracts;
using VyazmaTech.Prachka.Application.Mapping;
using VyazmaTech.Prachka.Domain.Core.ValueObjects;
using static VyazmaTech.Prachka.Application.Contracts.Core.Queues.Commands.ChangeQueueActivityBoundaries;

namespace VyazmaTech.Prachka.Application.Handlers.Core.Queue.Commands.ChangeQueueActivityBoundaries;

internal sealed class ChangeQueueActivityBoundariesCommandHandler : ICommandHandler<Command, Response>
{
    private readonly IPersistenceContext _persistenceContext;

    public ChangeQueueActivityBoundariesCommandHandler(IPersistenceContext persistenceContext)
    {
        _persistenceContext = persistenceContext;
    }

    public async ValueTask<Response> Handle(Command request, CancellationToken cancellationToken)
    {
        Domain.Core.Queues.Queue queue = await _persistenceContext.Queues
            .GetByIdAsync(request.QueueId, cancellationToken);

        var activityBoundaries = QueueActivityBoundaries.Create(
            request.ActiveFrom,
            request.ActiveUntil);

        queue.ChangeActivityBoundaries(activityBoundaries);

        await _persistenceContext.SaveChangesAsync(cancellationToken);

        return new Response(queue.ToDto());
    }
}