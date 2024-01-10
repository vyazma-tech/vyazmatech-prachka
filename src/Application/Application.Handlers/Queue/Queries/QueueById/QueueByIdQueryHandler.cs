using Application.Core.Contracts;
using Application.DataAccess.Contracts;
using Domain.Common.Result;
using Domain.Core.Queue;
using Infrastructure.DataAccess.Contracts;
using Infrastructure.DataAccess.Specifications.Queue;
using static Application.Handlers.Queue.Queries.QueueById.QueueByIdQuery;

namespace Application.Handlers.Queue.Queries.QueueById;

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
            .FindByAsync(
                new QueueByIdSpecification(request.Id),
                cancellationToken);

        if (result.IsFaulted)
            return new Result<Response>(result.Error);

        QueueEntity queue = result.Value;

        return queue.ToDto();
    }
}