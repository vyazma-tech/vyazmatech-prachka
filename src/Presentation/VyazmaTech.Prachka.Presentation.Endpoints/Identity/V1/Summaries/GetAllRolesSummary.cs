using FastEndpoints;
using VyazmaTech.Prachka.Application.Dto.Identity;

namespace VyazmaTech.Prachka.Presentation.Endpoints.Identity.V1.Summaries;

internal sealed class GetAllRolesSummary : Summary<GetAllRolesEndpoint>
{
    public GetAllRolesSummary()
    {
        Summary = "get a list of all roles in authentication system";
        Response<AllRolesDto>();
    }
}