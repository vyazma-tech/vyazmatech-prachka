using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VyazmaTech.Prachka.Application.Dto.User;
using ProblemDetails = FastEndpoints.ProblemDetails;

namespace VyazmaTech.Prachka.Presentation.Endpoints.User.Summaries;

internal sealed class FindUserByIdSummary : Summary<FindUserByIdEndpoint>
{
    public FindUserByIdSummary()
    {
        Summary = "Find user by id";
        Response<UserDto>();
        Response<ProblemDetails>(StatusCodes.Status404NotFound, "User with specified id was not found");
        Response<ValidationProblemDetails>(StatusCodes.Status400BadRequest, "Id was not in correct format");
    }
}