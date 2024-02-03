using Application.Core.Contracts.Common;

namespace Application.Core.Contracts.Identity.Queries;

public static class GetAllRoles
{
    public record Query() : IQuery<Response>;

    public record struct Response(string[] Roles);
}