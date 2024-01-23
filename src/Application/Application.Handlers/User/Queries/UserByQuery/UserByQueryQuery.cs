using Application.Core.Common;
using Application.Core.Contracts;
using Domain.Core.User;

namespace Application.Handlers.User.Queries.UserByQuery;

public static class UserByQueryQuery
{
    public record Query(
        string? TelegramId,
        string? Fullname,
        DateOnly? RegistrationDate,
        int Page) : IQuery<PagedResponse<Response>>;

    public record struct Response(
        Guid Id,
        string TelegramId,
        string Fullname,
        DateTime? ModifiedOn,
        DateOnly CreationDate);

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