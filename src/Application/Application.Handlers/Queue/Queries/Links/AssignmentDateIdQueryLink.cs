using Application.Core.Querying.Abstractions;
using Application.Handlers.Queue.Queries;
using Domain.Core.Queue;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.Queue.Queries.Links;

// TODO: filter
public class AssignmentDateQueryLink : QueryLinkBase<IQueryable<QueueEntity>, QueueQuery>
{
    private readonly ILogger<AssignmentDateQueryLink> _logger;

    public AssignmentDateQueryLink(ILogger<AssignmentDateQueryLink> logger)
    {
        _logger = logger;
    }

    protected override IQueryable<QueueEntity> TryApply(IQueryable<QueueEntity> requestQueryable, QueueQuery requestParameter)
    {
        return null!;
    }
}