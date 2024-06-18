using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.Dto.Core.User;

namespace VyazmaTech.Prachka.Application.Contracts.Core.Users.Commands;

public static class CreateUser
{
    public record struct Command(Guid Id, string Fullname, string TelegramUsername)
        : IValidatableRequest<Response>;

    public record struct Response(UserDto User);
}