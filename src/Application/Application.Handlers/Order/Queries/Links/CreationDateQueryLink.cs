using Infrastructure.DataAccess.Quering.Abstractions;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.Order.Queries.Links;

public class CreationDateQueryLink : QueryLinkBase<OrderQuery.QueryBuilder, OrderQueryParameter>
{
    private readonly ILogger<OrderIdQueryLink> _logger;

    public CreationDateQueryLink(ILogger<OrderIdQueryLink> logger)
    {
        _logger = logger;
    }

    protected override OrderQuery.QueryBuilder? TryApply(
        OrderQuery.QueryBuilder requestQueryBuilder,
        QueryParameter<OrderQueryParameter> requestParameter)
    {
        if (requestParameter.Type is not OrderQueryParameter.CreationDate)
            return null;

        try
        {
            return requestQueryBuilder
                .WithCreationDate(DateOnly.Parse(requestParameter.Pattern));
        }
        catch (Exception e)
        {
            _logger.LogError("Unable to convert {Pattern} to date time", requestParameter.Pattern);
            throw new Exception($"Unable to convert {requestParameter.Pattern} to date time", e);
        }
    }
}