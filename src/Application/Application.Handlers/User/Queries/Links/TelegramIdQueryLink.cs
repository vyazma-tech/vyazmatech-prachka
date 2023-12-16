using Application.Handlers.User.Queries;
using Infrastructure.DataAccess.Quering.Abstractions;
using Microsoft.Extensions.Logging;

namespace Application.Handlers.User.Queries.Links;

public class TelegramIdQueryLink : QueryLinkBase<UserQuery.QueryBuilder, UserQueryParameter>
{
    protected override UserQuery.QueryBuilder? TryApply(
        UserQuery.QueryBuilder requestQueryBuilder,
        QueryParameter<UserQueryParameter> requestParameter)
    {
        if (requestParameter.Type is not UserQueryParameter.TelegramId)
            return null;

        return requestQueryBuilder.WithTelegramId(requestParameter.Pattern);
    }
}