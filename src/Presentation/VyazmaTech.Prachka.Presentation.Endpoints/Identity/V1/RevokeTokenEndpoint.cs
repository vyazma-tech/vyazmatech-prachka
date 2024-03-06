using FastEndpoints;
using Mediator;
using VyazmaTech.Prachka.Application.Contracts.Identity.Commands;
using VyazmaTech.Prachka.Presentation.Endpoints.Identity.V1.Models;

namespace VyazmaTech.Prachka.Presentation.Endpoints.Identity.V1;

internal class RevokeTokenEndpoint : Endpoint<RevokeTokenRequest>
{
    private readonly ISender _sender;

    public RevokeTokenEndpoint(ISender sender)
    {
        _sender = sender;
    }

    public override void Configure()
    {
        Delete("revoke");
        AllowAnonymous();
        Group<IdentityEndpointGroup>();
        Version(1);
    }

    public override async Task HandleAsync(RevokeTokenRequest req, CancellationToken ct)
    {
        var command = new RevokeToken.Command(req.Username);

        RevokeToken.Response response = await _sender.Send(command, ct);

        await SendNoContentAsync(ct);
    }
}