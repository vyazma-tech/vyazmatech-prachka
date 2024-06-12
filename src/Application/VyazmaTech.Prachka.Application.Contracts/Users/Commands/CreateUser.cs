using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.Dto.User;
using VyazmaTech.Prachka.Domain.Common.Result;

namespace VyazmaTech.Prachka.Application.Contracts.Users.Commands;

public static class CreateUser
{
    public record struct Command(Guid Id, string Fullname, string TelegramUsername)
        : IValidatableRequest<Result<Response>>;

    public record struct Response(UserDto User);
}