using Application.Handlers.Order.Queries;
using Infrastructure.DataAccess.Quering.Abstractions;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.Queue.Queries.Links;

public class OrderIdQueryLink : QueryLinkBase<QueueQuery.QueryBuilder, QueueQueryParameter>
{
    private readonly ILogger<OrderIdQueryLink> _logger;

    public OrderIdQueryLink(ILogger<OrderIdQueryLink> logger)
    {
        _logger = logger;
    }

    protected override QueueQuery.QueryBuilder? TryApply(
        QueueQuery.QueryBuilder requestQueryBuilder,
        QueryParameter<QueueQueryParameter> requestParameter)
    {
        if (requestParameter.Type is not QueueQueryParameter.OrderId)
            return null;

        try
        {
            return requestQueryBuilder.WithOrderId(Guid.Parse(requestParameter.Pattern));
        }
        catch (Exception e)
        {
            _logger.LogError("Unable to convert {Pattern} to guid", requestParameter.Pattern);
            throw new Exception($"Unable to convert {requestParameter.Pattern} to guid", e);
        }
    }
}