using FastEndpoints;
using Microsoft.AspNetCore.Http;
using VyazmaTech.Prachka.Application.Contracts.Identity.Commands;

namespace VyazmaTech.Prachka.Presentation.Endpoints.Identity.V1.Summaries;

internal sealed class ChangeRoleSummary : Summary<ChangeRoleEndpoint>
{
    public ChangeRoleSummary()
    {
        Summary = "changes role of specified user";
        Response<ChangeRole.Response>();
        Response(
            StatusCodes.Status403Forbidden,
            "Current identity user is not allowed to perform this operation");
    }
}