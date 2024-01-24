using Application.Core.Common;
using Application.Core.Contracts.Common;
using Domain.Core.User;

namespace Application.Core.Contracts.Users.Queries;

public static class UserByQuery
{
    public record Query(
        string? TelegramId,
        string? Fullname,
        DateOnly? RegistrationDate,
        int? Page) : IQuery<PagedResponse<Response>>;

    public record struct Response(
        Guid Id,
        string TelegramId,
        string Fullname,
        DateTime? ModifiedOn,
        DateOnly RegistrationDate);

    public static Response ToDto(this UserEntity user)
    {
        return new Response(
            user.Id,
            user.TelegramId,
            user.Fullname,
            user.ModifiedOn?.Value,
            user.CreationDate);
    }
}