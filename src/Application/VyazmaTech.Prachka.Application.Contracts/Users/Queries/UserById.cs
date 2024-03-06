using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.Dto.User;
using VyazmaTech.Prachka.Domain.Common.Result;

namespace VyazmaTech.Prachka.Application.Contracts.Users.Queries;

public static class UserById
{
    public record struct Query(Guid Id) : IQuery<Result<Response>>;

    public record struct Response(UserDto User);
}