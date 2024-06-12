using VyazmaTech.Prachka.Application.Contracts.Common;

namespace VyazmaTech.Prachka.Application.Contracts.Identity.Commands;

public static class ChangeRole
{
    public record struct Command(string TelegramUsername, string NewRoleName) : IValidatableRequest<Response>;

    public record struct Response;
}