using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.DataAccess.Contracts;
using VyazmaTech.Prachka.Application.Mapping;
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
        Domain.Core.Queues.Queue queue = await _context.Queues
            .GetByIdAsync(request.Id, cancellationToken);

        return new Response(queue.ToQueueWithOrdersDto());
    }
}