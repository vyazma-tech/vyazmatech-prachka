using Application.Core.Contracts.Common;
using Domain.Common.Result;

namespace Application.Core.Contracts.Identity.Commands;

public static class RevokeToken
{
    public record Command(string TelegramUsername) : IValidatableRequest<Response>;

    public record struct Response(Result Result);
}