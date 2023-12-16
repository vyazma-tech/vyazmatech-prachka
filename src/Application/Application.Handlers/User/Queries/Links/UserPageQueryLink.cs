using Infrastructure.DataAccess.Quering.Abstractions;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.User.Queries.Links;

public class UserPageQueryLink : QueryLinkBase<UserQuery.QueryBuilder, UserQueryParameter>
{
    private readonly ILogger<UserPageQueryLink> _logger;

    public UserPageQueryLink(ILogger<UserPageQueryLink> logger)
    {
        _logger = logger;
    }

    protected override UserQuery.QueryBuilder? TryApply(UserQuery.QueryBuilder requestQueryBuilder,
        QueryParameter<UserQueryParameter> requestParameter)
    {
        if (requestParameter.Type is not UserQueryParameter.Page)
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