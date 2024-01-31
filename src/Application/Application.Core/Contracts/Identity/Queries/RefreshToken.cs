using Application.Core.Contracts.Common;
using Application.Dto.Identity;
using Domain.Common.Result;

namespace Application.Core.Contracts.Identity.Queries;

public static class RefreshToken
{
    public record Query(string AccessToken, string RefreshToken) : IQuery<Result<Response>>;

    public record struct Response(IdentityTokenDto Tokens);
}