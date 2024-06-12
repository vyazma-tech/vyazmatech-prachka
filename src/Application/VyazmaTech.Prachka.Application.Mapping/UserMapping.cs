using VyazmaTech.Prachka.Application.Dto;
using VyazmaTech.Prachka.Application.Dto.User;
using VyazmaTech.Prachka.Domain.Core.Users;

namespace VyazmaTech.Prachka.Application.Mapping;

public static class UserMapping
{
    public static UserDto ToDto(this User user)
    {
        return new UserDto(
            user.Id,
            user.TelegramUsername,
            user.Fullname,
            user.ModifiedOnUtc,
            user.CreationDate);
    }

    public static PagedResponse<UserDto> ToPagedResponse(
        this IEnumerable<UserDto> orders,
        int currentPage,
        long totalPages,
        int recordsPerPage)
    {
        return new PagedResponse<UserDto>
        {
            Bunch = orders.ToArray(),
            CurrentPage = currentPage,
            TotalPages = totalPages,
            RecordPerPage = recordsPerPage,
        };
    }
}