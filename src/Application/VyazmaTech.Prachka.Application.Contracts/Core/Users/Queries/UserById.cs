using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.Dto.Core.User;

namespace VyazmaTech.Prachka.Application.Contracts.Core.Users.Queries;

public static class UserById
{
    public record struct Query(Guid Id) : IQuery<Response>;

    public record struct Response(UserDto User);
}