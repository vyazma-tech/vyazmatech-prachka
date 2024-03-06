using FastEndpoints;
using Microsoft.AspNetCore.Http;

namespace VyazmaTech.Prachka.Presentation.Endpoints.Identity.V1.Summaries;

internal sealed class RevokeTokenSummary : Summary<RevokeTokenEndpoint>
{
    public RevokeTokenSummary()
    {
        Summary = "deletes refresh token for a user";
        Response(StatusCodes.Status204NoContent);
    }
}