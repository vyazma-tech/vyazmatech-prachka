using Application.Core.Contracts;
using Domain.Common.Result;
using Domain.Core.User;

namespace Application.Handlers.User.Queries.UserById;

public static class UserByIdQuery
{
    public record Query(Guid Id) : IQuery<Result<Response>>;

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
            user.TelegramId.Value,
            user.Fullname.Value,
            user.ModifiedOn?.Value,
            user.CreationDate);
    }
}