using Application.Core.Querying.Abstractions;
using Application.Handlers.Order.Queries;
using Domain.Core.Queue;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.Queue.Queries.Links;

// TODO: move to order
public class OrderIdQueryLink : QueryLinkBase<IQueryable<QueueEntity>, QueueQuery>
{
    private readonly ILogger<OrderIdQueryLink> _logger;

    public OrderIdQueryLink(ILogger<OrderIdQueryLink> logger)
    {
        _logger = logger;
    }

    protected override IQueryable<QueueEntity> TryApply(
        IQueryable<QueueEntity> requestQueryable,
        QueueQuery requestParameter)
    {
        return null!;
    }
}