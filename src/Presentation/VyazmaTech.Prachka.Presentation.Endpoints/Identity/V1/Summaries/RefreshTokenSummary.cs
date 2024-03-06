using FastEndpoints;
using Microsoft.AspNetCore.Http;
using VyazmaTech.Prachka.Application.Dto.Identity;
using ProblemDetails = Microsoft.AspNetCore.Mvc.ProblemDetails;

namespace VyazmaTech.Prachka.Presentation.Endpoints.Identity.V1.Summaries;

internal sealed class RefreshTokenSummary : Summary<RefreshTokenEndpoint>
{
    public RefreshTokenSummary()
    {
        Summary = "refreshes current user access token";
        Response<IdentityTokenDto>();
        Response<ProblemDetails>(StatusCodes.Status401Unauthorized, "Refresh token expired");
        Response<ProblemDetails>(StatusCodes.Status404NotFound, "User does not exist");
    }
}