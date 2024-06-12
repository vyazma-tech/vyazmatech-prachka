using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.Core.Specifications;
using VyazmaTech.Prachka.Application.DataAccess.Contracts;
using VyazmaTech.Prachka.Application.Mapping;
using VyazmaTech.Prachka.Domain.Common.Result;
using VyazmaTech.Prachka.Domain.Core.Queue;
using static VyazmaTech.Prachka.Application.Contracts.Queues.Queries.QueueById;

namespace VyazmaTech.Prachka.Application.Handlers.Queue.Queries;

internal sealed class QueueByIdQueryHandler : IQueryHandler<Query, Result<Response>>
{
    private readonly IPersistenceContext _context;

    public QueueByIdQueryHandler(IPersistenceContext context)
    {
        _context = context;
    }

    public async ValueTask<Result<Response>> Handle(Query request, CancellationToken cancellationToken)
    {
        Result<QueueEntity> result = await _context.Queues
            .FindByIdAsync(request.Id, cancellationToken);

        if (result.IsFaulted)
        {
            return new Result<Response>(result.Error);
        }

        QueueEntity queue = result.Value;

        return new Response(queue.ToQueueWithOrdersDto());
    }
}