using VyazmaTech.Prachka.Application.Abstractions.Identity.Models;
using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.Dto.Identity;
using static VyazmaTech.Prachka.Application.Contracts.Identity.Queries.GetAllRoles;

namespace VyazmaTech.Prachka.Application.Handlers.Identity.Queries;

internal sealed class GetAllRolesQueryHandler : IQueryHandler<Query, Response>
{
    private static ValueTask<Response> Response => ValueTask.FromResult(new Response(new AllRolesDto(new[]
    {
        VyazmaTechRoleNames.AdminRoleName,
        VyazmaTechRoleNames.ModeratorRoleName,
        VyazmaTechRoleNames.EmployeeRoleName,
        VyazmaTechRoleNames.UserRoleName,
    })));

    public ValueTask<Response> Handle(Query request, CancellationToken cancellationToken)
        => Response;
}