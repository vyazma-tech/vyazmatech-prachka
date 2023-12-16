using Infrastructure.DataAccess.Quering.Abstractions;

namespace Application.Handlers.User.Queries.Links;

public class FullnameQueryLink : QueryLinkBase<UserQuery.QueryBuilder, UserQueryParameter>
{
    protected override UserQuery.QueryBuilder? TryApply(
        UserQuery.QueryBuilder requestQueryBuilder,
        QueryParameter<UserQueryParameter> requestParameter)
    {
        if (requestParameter.Type is not UserQueryParameter.Fullname)
            return null;

        return requestQueryBuilder.WithFullname(requestParameter.Pattern);
    }
}