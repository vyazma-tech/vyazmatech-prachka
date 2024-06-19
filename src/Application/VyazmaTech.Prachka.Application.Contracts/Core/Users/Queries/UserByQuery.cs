using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.Dto;
using VyazmaTech.Prachka.Application.Dto.Core.User;

namespace VyazmaTech.Prachka.Application.Contracts.Core.Users.Queries;

public static class UserByQuery
{
    public record struct Query(
        string? TelegramUsername,
        string? Fullname,
        DateOnly? RegistrationDate,
        int Page,
        int Limit) : IQuery<Response>;

    public record struct Response(PagedResponse<UserDto> Users);
}