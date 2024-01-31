using Application.Core.Contracts.Common;
using Application.DataAccess.Contracts.Abstractions;
using Application.Dto.Identity;
using Domain.Common.Result;

namespace Application.Core.Contracts.Identity.Commands;

public static class CreateUser
{
    public record Command(Guid Id, IdentityUserCredentials Credentials, string Role) : IValidatableRequest<Result<Response>>;

    public record struct Response(IdentityUserDto User, IdentityTokenDto Tokens);
}