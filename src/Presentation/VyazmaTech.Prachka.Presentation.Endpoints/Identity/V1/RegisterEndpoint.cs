using FastEndpoints;
using Mediator;
using VyazmaTech.Prachka.Application.Abstractions.Identity.Models;
using VyazmaTech.Prachka.Application.Contracts.Identity.Commands;
using VyazmaTech.Prachka.Domain.Common.Exceptions;
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
        // TODO: переписать
        var registerCommand = new RegisterUser.Command(
            Guid.NewGuid(),
            new IdentityUserCredentials(
                req.TelegramUsername,
                req.Fullname,
                req.TelegramId,
                req.TelegramImageUrl),
            req.Role);

        try
        {
            RegisterUser.Response registerResponse = await _sender.Send(registerCommand, ct);

            var registrationResponse = new RegisterResponse(
                registerResponse.User.Id,
                registerResponse.User.Fullname,
                registerResponse.Tokens.Role,
                registerResponse.User.TelegramUsername,
                registerResponse.User.TelegramImageUrl,
                registerResponse.Tokens.AccessToken,
                registerResponse.Tokens.RefreshToken);

            await SendOkAsync(registrationResponse, ct);
        }
        catch (IdentityException e)
        {
            await this.SendProblemsAsync(e.ToProblemDetails());
        }
    }
}