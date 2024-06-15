using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.Dto;
using VyazmaTech.Prachka.Application.Dto.User;

namespace VyazmaTech.Prachka.Application.Contracts.Users.Queries;

public static class UserByQuery
{
    public record struct Query(
        string? TelegramUsername,
        string? Fullname,
        DateOnly? RegistrationDate,
        int Page) : IQuery<Response>;

    public record struct Response(PagedResponse<UserDto> Users);
}