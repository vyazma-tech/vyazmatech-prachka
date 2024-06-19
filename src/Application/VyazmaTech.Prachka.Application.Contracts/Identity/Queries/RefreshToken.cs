using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.Dto.Identity;

namespace VyazmaTech.Prachka.Application.Contracts.Identity.Queries;

public static class RefreshToken
{
    public record struct Query(string AccessToken, string RefreshToken) : IQuery<Response>;

    public record struct Response(IdentityTokenDto Tokens);
}