using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VyazmaTech.Prachka.Presentation.Endpoints.Identity.V1.Models;

namespace VyazmaTech.Prachka.Presentation.Endpoints.Identity.V1.Summaries;

internal sealed class RegisterSummary : Summary<RegisterEndpoint>
{
    public RegisterSummary()
    {
        Summary = "registers user in authentication system using tg credentials";
        Response<RegisterResponse>(StatusCodes.Status202Accepted);
        Response<ValidationProblemDetails>(StatusCodes.Status400BadRequest, "Invalid credentials");
    }
}