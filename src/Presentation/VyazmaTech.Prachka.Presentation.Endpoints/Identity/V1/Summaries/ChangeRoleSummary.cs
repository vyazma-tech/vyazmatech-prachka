using FastEndpoints;
using Microsoft.AspNetCore.Http;
using VyazmaTech.Prachka.Domain.Common.Result;

namespace VyazmaTech.Prachka.Presentation.Endpoints.Identity.V1.Summaries;

internal sealed class ChangeRoleSummary : Summary<ChangeRoleEndpoint>
{
    public ChangeRoleSummary()
    {
        Summary = "changes role of specified user";
        Response<Result>();
        Response<Result>(
            StatusCodes.Status403Forbidden,
            "Current identity user is not allowed to perform this operation");
    }
}