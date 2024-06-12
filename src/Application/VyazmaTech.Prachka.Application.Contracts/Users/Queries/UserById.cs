using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.Dto.User;

namespace VyazmaTech.Prachka.Application.Contracts.Users.Queries;

public static class UserById
{
    public record struct Query(Guid Id) : IQuery<Response>;

    public record struct Response(UserDto User);
}