using VyazmaTech.Prachka.Application.Abstractions.Identity.Models;
using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.Dto.Identity;

namespace VyazmaTech.Prachka.Application.Contracts.Identity.Commands;

public static class RegisterUser
{
    public record struct Command(
        Guid Id,
        IdentityUserCredentials Credentials,
        string Role) : IValidatableRequest<Response>;

    public record struct Response(IdentityUserDto User, IdentityTokenDto Tokens);
}