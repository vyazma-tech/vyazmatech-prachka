using VyazmaTech.Prachka.Application.Abstractions.Identity.Models;
using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.Dto.Identity;
using VyazmaTech.Prachka.Domain.Common.Result;

namespace VyazmaTech.Prachka.Application.Contracts.Identity.Commands;

public static class CreateUser
{
    public record struct Command(Guid Id, IdentityUserCredentials Credentials, string Role) : IValidatableRequest<Result<Response>>;

    public record struct Response(IdentityUserDto User, IdentityTokenDto Tokens);
}