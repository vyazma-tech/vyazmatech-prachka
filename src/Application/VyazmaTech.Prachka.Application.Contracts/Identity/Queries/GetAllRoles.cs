using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.Dto.Identity;

namespace VyazmaTech.Prachka.Application.Contracts.Identity.Queries;

public static class GetAllRoles
{
    public record struct Query : IQuery<Response>;

    public record struct Response(AllRolesDto Roles);
}