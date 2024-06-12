using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.Core.Specifications;
using VyazmaTech.Prachka.Application.DataAccess.Contracts;
using VyazmaTech.Prachka.Application.Mapping;
using VyazmaTech.Prachka.Domain.Core.Queue;
using static VyazmaTech.Prachka.Application.Contracts.Queues.Queries.QueueById;

namespace VyazmaTech.Prachka.Application.Handlers.Queue.Queries;

internal sealed class QueueByIdQueryHandler : IQueryHandler<Query, Response>
{
    private readonly IPersistenceContext _context;

    public QueueByIdQueryHandler(IPersistenceContext context)
    {
        _context = context;
    }

    public async ValueTask<Response> Handle(Query request, CancellationToken cancellationToken)
    {
        QueueEntity queue = await _context.Queues
            .FindByIdAsync(request.Id, cancellationToken);

        return new Response(queue.ToQueueWithOrdersDto());
    }
}