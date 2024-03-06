using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.Dto.Identity;

namespace VyazmaTech.Prachka.Application.Contracts.Identity.Queries;

public static class ValidateToken
{
    public record struct Query(string AccessToken) : IQuery<Response>;

    public record struct Response(PrincipalDto Principal);
}