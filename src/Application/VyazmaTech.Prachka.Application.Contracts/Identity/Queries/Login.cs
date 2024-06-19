using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.Dto.Identity;

namespace VyazmaTech.Prachka.Application.Contracts.Identity.Queries;

public static class Login
{
    public record struct Query(string Username) : IQuery<Response>;

    public record struct Response(IdentityTokenDto Tokens);
}