using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VyazmaTech.Prachka.Application.Dto.Identity;

namespace VyazmaTech.Prachka.Presentation.Endpoints.Identity.V1.Summaries;

internal sealed class LoginSummary : Summary<LoginEndpoint>
{
    public LoginSummary()
    {
        Summary = "performs login for a user";
        Response<IdentityTokenDto>();
        Response<ValidationProblemDetails>(StatusCodes.Status400BadRequest, "Invalid input");
    }
}