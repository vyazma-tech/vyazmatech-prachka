using Application.Core.Contracts.Common;
using Domain.Common.Result;

namespace Application.Core.Contracts.Identity.Commands;

public static class ChangeRole
{
    public record Command(string TelegramUsername, string NewRoleName) : IValidatableRequest<Response>;

    public record struct Response(Result Result);
}