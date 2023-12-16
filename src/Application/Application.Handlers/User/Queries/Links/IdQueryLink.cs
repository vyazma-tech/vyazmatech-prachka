using Application.Handlers.User.Queries;
using Infrastructure.DataAccess.Quering.Abstractions;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.User.Queries.Links;

public class IdQueryLink : QueryLinkBase<UserQuery.QueryBuilder, UserQueryParameter>
{
    private readonly ILogger<IdQueryLink> _logger;

    public IdQueryLink(ILogger<IdQueryLink> logger)
    {
        _logger = logger;
    }

    protected override UserQuery.QueryBuilder? TryApply(
        UserQuery.QueryBuilder requestQueryBuilder,
        QueryParameter<UserQueryParameter> requestParameter)
    {
        if (requestParameter.Type is not UserQueryParameter.Id)
            return null;

        try
        {
            return requestQueryBuilder.WithId(Guid.Parse(requestParameter.Pattern));
        }
        catch (Exception e)
        {
            _logger.LogError("Unable to convert {Pattern} to guid", requestParameter.Pattern);
            throw new Exception($"Unable to convert {requestParameter.Pattern} to guid", e);
        }
    }
}