using Application.Core.Querying.Abstractions;
using Application.Handlers.Queue.Queries;
using Domain.Core.Queue;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.Queue.Queries.Links;

public class QueueIdQueryLink : QueryLinkBase<IQueryable<QueueEntity>, QueueQuery>
{
    private readonly ILogger<QueueIdQueryLink> _logger;

    public QueueIdQueryLink(ILogger<QueueIdQueryLink> logger)
    {
        _logger = logger;
    }

    protected override IQueryable<QueueEntity> TryApply(
        IQueryable<QueueEntity> requestQueryable,
        QueueQuery requestParameter)
    {
        if (requestParameter.QueueId.Equals(Guid.Empty))
            return requestQueryable;

        requestQueryable = requestQueryable
            .Where(x => x.Id.Equals(requestParameter.QueueId));

        return requestQueryable;
    }
}