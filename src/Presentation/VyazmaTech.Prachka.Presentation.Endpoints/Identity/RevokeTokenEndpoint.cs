using FastEndpoints;
using Mediator;
using VyazmaTech.Prachka.Application.Contracts.Identity.Commands;
using VyazmaTech.Prachka.Application.Dto.Identity;
using VyazmaTech.Prachka.Presentation.Endpoints.Identity.Models;

namespace VyazmaTech.Prachka.Presentation.Endpoints.Identity;

internal class RevokeTokenEndpoint : Endpoint<RevokeTokenRequest, IdentityTokenDto>
{
    private readonly ISender _sender;

    public RevokeTokenEndpoint(ISender sender)
    {
        _sender = sender;
    }

    public override void Configure()
    {
        Delete("api/identity/revoke");
        AllowAnonymous();
    }

    public override async Task HandleAsync(RevokeTokenRequest req, CancellationToken ct)
    {
        var command = new RevokeToken.Command(req.Username);

        RevokeToken.Response response = await _sender.Send(command, ct);

        await SendNoContentAsync(ct);
    }
}