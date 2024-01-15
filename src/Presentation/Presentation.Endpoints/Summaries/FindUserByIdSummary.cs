using FastEndpoints;
using Presentation.Endpoints.User.FindUsers;
using static Application.Handlers.User.Queries.UserById.UserByIdQuery;

namespace Presentation.Endpoints.Summaries;

internal class FindUserByIdSummary : Summary<FindUserByIdEndpoint, Query>
{
    public FindUserByIdSummary()
    {
        Summary = "Provide searching user by id";
        RequestParam(
            r => r.Id,
            "the ID must belong to an existing user");
        Response(404, "Error: User not found");
    }
}