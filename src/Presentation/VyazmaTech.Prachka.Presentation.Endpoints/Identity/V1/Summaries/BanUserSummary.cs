using FastEndpoints;
using Microsoft.AspNetCore.Http;
using VyazmaTech.Prachka.Application.Contracts.Identity.Commands;

namespace VyazmaTech.Prachka.Presentation.Endpoints.Identity.V1.Summaries;

internal sealed class BanUserSummary : Summary<BanUserEndpoint>
{
    public BanUserSummary()
    {
        Summary = "ban user by username";
        Response<ChangeRole.Response>();
        Response(
            StatusCodes.Status403Forbidden,
            "Current identity user is not allowed to perform this operation");
        Response(
            StatusCodes.Status401Unauthorized,
            "You are not logged in");
    }
}