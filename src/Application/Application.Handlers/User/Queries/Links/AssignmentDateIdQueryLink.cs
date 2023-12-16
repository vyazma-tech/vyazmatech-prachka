using Infrastructure.DataAccess.Quering.Abstractions;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.User.Queries.Links;

public class RegistrationDateQueryLink : QueryLinkBase<UserQuery.QueryBuilder, UserQueryParameter>
{
    private readonly ILogger<RegistrationDateQueryLink> _logger;

    public RegistrationDateQueryLink(ILogger<RegistrationDateQueryLink> logger)
    {
        _logger = logger;
    }

    protected override UserQuery.QueryBuilder? TryApply(
        UserQuery.QueryBuilder requestQueryBuilder,
        QueryParameter<UserQueryParameter> requestParameter)
    {
        if (requestParameter.Type is not UserQueryParameter.RegistrationDate)
            return null;

        try
        {
            return requestQueryBuilder.WithRegistrationDate(
                DateOnly.Parse(requestParameter.Pattern));
        }
        catch (Exception e)
        {
            _logger.LogError("Unable to convert {Pattern} to date time", requestParameter.Pattern);
            throw new Exception($"Unable to convert {requestParameter.Pattern} to guid", e);
        }
    }
}