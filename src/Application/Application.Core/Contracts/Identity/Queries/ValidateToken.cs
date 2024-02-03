using System.Security.Claims;
using Application.Core.Contracts.Common;

namespace Application.Core.Contracts.Identity.Queries;

public static class ValidateToken
{
    public record Query(string AccessToken) : IQuery<Response>;

    public record struct Response(ClaimsPrincipal? Principal);
}