using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Endpoints.User.FindUsers;
using static Application.Core.Contracts.Users.Queries.UserById;
using ProblemDetails = FastEndpoints.ProblemDetails;

namespace Presentation.Endpoints.User.Summaries;

internal sealed class FindUserByIdSummary : Summary<FindUserByIdEndpoint>
{
    public FindUserByIdSummary()
    {
        Summary = "Find user by id";
        Response<Response>();
        Response<ProblemDetails>(StatusCodes.Status404NotFound, "User with specified id was not found");
        Response<ValidationProblemDetails>(StatusCodes.Status400BadRequest, "Id was not in correct format");
    }
}