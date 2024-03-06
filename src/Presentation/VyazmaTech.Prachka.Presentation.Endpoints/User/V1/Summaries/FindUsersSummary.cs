using FastEndpoints;
using Microsoft.AspNetCore.Http;
using VyazmaTech.Prachka.Application.Dto;
using VyazmaTech.Prachka.Application.Dto.User;

namespace VyazmaTech.Prachka.Presentation.Endpoints.User.V1.Summaries;

internal sealed class FindUsersSummary : Summary<FindUsersEndpoint>
{
    public FindUsersSummary()
    {
        Summary = "Find users by query";
        Description = @"Returns a bunch of 10 users by the application defaults from the first page.
                              Default page is 0.";
        Response<PagedResponse<UserDto>>(StatusCodes.Status206PartialContent);
        Response(StatusCodes.Status204NoContent, "Nothing found on specified page.");
        Response<ProblemDetails>(StatusCodes.Status400BadRequest, "Input was not in correct format.");
    }
}