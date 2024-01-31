using Application.Core.Contracts.Common;
using Application.DataAccess.Contracts.Abstractions;
using static Application.Core.Contracts.Identity.Queries.GetAllRoles;

namespace Application.Handlers.Identity.Queries;

internal sealed class GetAllRolesQueryHandler : IQueryHandler<Query, Response>
{
    private static ValueTask<Response> Response => ValueTask.FromResult(new Response(new[]
    {
        VyazmaTechRoleNames.AdminRoleName,
        VyazmaTechRoleNames.ModeratorRoleName,
        VyazmaTechRoleNames.EmployeeRoleName,
        VyazmaTechRoleNames.UserRoleName,
    }));

    public ValueTask<Response> Handle(Query request, CancellationToken cancellationToken)
        => Response;
}