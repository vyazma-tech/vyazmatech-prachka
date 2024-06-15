using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.DataAccess.Contracts;
using VyazmaTech.Prachka.Application.Mapping;
using VyazmaTech.Prachka.Domain.Core.ValueObjects;
using VyazmaTech.Prachka.Domain.Kernel;
using static VyazmaTech.Prachka.Application.Contracts.Queues.Commands.ChangeQueueActivityBoundaries;

namespace VyazmaTech.Prachka.Application.Handlers.Queue.Commands.ChangeQueueActivityBoundaries;

internal sealed class ChangeQueueActivityBoundariesCommandHandler : ICommandHandler<Command, Response>
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

    public async ValueTask<Response> Handle(Command request, CancellationToken cancellationToken)
    {
        Domain.Core.Queues.Queue queue = await _persistenceContext.Queues
            .GetByIdAsync(request.QueueId, cancellationToken);

        var activityBoundaries = QueueActivityBoundaries.Create(
            request.ActiveFrom,
            request.ActiveUntil);

        queue.ChangeActivityBoundaries(activityBoundaries, _dateTimeProvider.UtcNow);

        await _persistenceContext.SaveChangesAsync(cancellationToken);

        return new Response(queue.ToDto());
    }
}