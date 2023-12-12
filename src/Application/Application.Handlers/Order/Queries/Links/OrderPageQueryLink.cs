using Infrastructure.DataAccess.Quering.Abstractions;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.Order.Queries.Links;

public class OrderPageQueryLink : QueryLinkBase<OrderQuery.QueryBuilder, OrderQueryParameter>
{
    private readonly ILogger<OrderPageQueryLink> _logger;

    public OrderPageQueryLink(ILogger<OrderPageQueryLink> logger)
    {
        _logger = logger;
    }

    protected override OrderQuery.QueryBuilder? TryApply(OrderQuery.QueryBuilder requestQueryBuilder,
        QueryParameter<OrderQueryParameter> requestParameter)
    {
        if (requestParameter.Type is not OrderQueryParameter.Page)
            return null;

        try
        {
            return requestQueryBuilder.WithPage(Convert.ToInt32(requestParameter.Pattern));
        }
        catch (Exception e)
        {
            _logger.LogError("Unable to convert {Pattern} to page number. Should be integer", requestParameter.Pattern);
            throw new Exception($"Unable to convert {requestParameter.Pattern} to page number. Should be integer", e);
        }
    }
}