using Infrastructure.DataAccess.Quering.Abstractions;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.Order.Queries.Links;

public class UserIdQueryLink : QueryLinkBase<OrderQuery.QueryBuilder, OrderQueryParameter>
{
    private readonly ILogger<OrderIdQueryLink> _logger;

    public UserIdQueryLink(ILogger<OrderIdQueryLink> logger)
    {
        _logger = logger;
    }

    protected override OrderQuery.QueryBuilder? TryApply(
        OrderQuery.QueryBuilder requestQueryBuilder,
        QueryParameter<OrderQueryParameter> requestParameter)
    {
        if (requestParameter.Type is not OrderQueryParameter.UserId)
            return null;

        try
        {
            return requestQueryBuilder.WithUserId(Guid.Parse(requestParameter.Pattern));
        }
        catch (Exception e)
        {
            _logger.LogError("Unable to convert {Pattern} to guid", requestParameter.Pattern);
            throw new Exception($"Unable to convert {requestParameter.Pattern} to guid", e);
        }
    }
}