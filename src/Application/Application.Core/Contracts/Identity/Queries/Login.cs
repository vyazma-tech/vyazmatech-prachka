using Application.Core.Contracts.Common;
using Application.Dto.Identity;
using Domain.Common.Result;

namespace Application.Core.Contracts.Identity.Queries;

public static class Login
{
    public record Query(string Username) : IQuery<Result<Response>>;

    public record struct Response(IdentityTokenDto Tokens);
}