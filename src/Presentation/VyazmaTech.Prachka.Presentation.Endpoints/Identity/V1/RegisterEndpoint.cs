using FastEndpoints;
using Mediator;
using VyazmaTech.Prachka.Application.Abstractions.Identity.Models;
using VyazmaTech.Prachka.Application.Contracts.Identity.Commands;
using VyazmaTech.Prachka.Domain.Common.Result;
using VyazmaTech.Prachka.Presentation.Endpoints.Extensions;
using VyazmaTech.Prachka.Presentation.Endpoints.Identity.V1.Models;

namespace VyazmaTech.Prachka.Presentation.Endpoints.Identity.V1;

internal class RegisterEndpoint : Endpoint<RegisterRequest, RegisterResponse>
{
    private readonly ISender _sender;

    public RegisterEndpoint(ISender sender)
    {
        _sender = sender;
    }

    public override void Configure()
    {
        Post("register");
        AllowAnonymous();
        Group<IdentityEndpointGroup>();
        Version(1);
    }

    public override async Task HandleAsync(RegisterRequest req, CancellationToken ct)
    {
        var registerCommand = new RegisterUser.Command(
            Guid.NewGuid(),
            new IdentityUserCredentials(
                req.TelegramUsername,
                req.Fullname,
                req.TelegramId,
                req.TelegramImageUrl),
            req.Role);

        Result<RegisterUser.Response> registerResponse = await _sender.Send(registerCommand, ct);

        if (registerResponse.IsFaulted)
        {
            await this.SendProblemsAsync(registerResponse.ToProblemDetails());
            return;
        }

        RegisterUser.Response identityUser = registerResponse.Value;

        var registrationResponse = new RegisterResponse(
            identityUser.User.Id,
            identityUser.User.Fullname,
            identityUser.User.Role,
            identityUser.User.TelegramUsername,
            identityUser.User.TelegramImageUrl,
            identityUser.Tokens.AccessToken,
            identityUser.Tokens.RefreshToken);

        await SendOkAsync(registrationResponse, ct);
    }
}