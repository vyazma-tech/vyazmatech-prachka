using VyazmaTech.Prachka.Application.Contracts.Common;

namespace VyazmaTech.Prachka.Application.Contracts.Identity.Commands;

public static class RevokeToken
{
    public record struct Command(string TelegramUsername) : IValidatableRequest<Response>;

    public record struct Response;
}