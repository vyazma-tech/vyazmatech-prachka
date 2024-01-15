using FastEndpoints;
using Presentation.Endpoints.User.FindUsers;

namespace Presentation.Endpoints.Summaries;

internal class FindUsersSummary : Summary<FindUsersEndpoint>
{
    public FindUsersSummary()
    {
        Summary = "list all users endpoint";
        Description = "lists all users from desired page with a bunch of 10 by application defaults";
        Response(206, "current page has no content");
    }
}