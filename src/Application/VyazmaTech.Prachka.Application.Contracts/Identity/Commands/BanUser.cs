using VyazmaTech.Prachka.Application.Contracts.Common;

namespace VyazmaTech.Prachka.Application.Contracts.Identity.Commands;

public static class BanUser
{
    public record struct Command(string Username) : IValidatableRequest<Response>;

    public record struct Response;
}