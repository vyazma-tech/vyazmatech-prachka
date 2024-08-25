using FastEndpoints;
using Mediator;
using VyazmaTech.Prachka.Application.Contracts.Identity.Commands;
using VyazmaTech.Prachka.Presentation.Authorization;
using VyazmaTech.Prachka.Presentation.Endpoints.Identity.V1.Models;

namespace VyazmaTech.Prachka.Presentation.Endpoints.Identity.V1;

internal sealed class BanUserEndpoint : Endpoint<BanUserRequest>
{
    private const string FeatureName = "BanUser";
    private readonly ISender _sender;

    public BanUserEndpoint(ISender sender)
    {
        _sender = sender;
    }

    public override void Configure()
    {
        Post("ban");
        Policies($"{AuthorizeFeatureAttribute.Prefix}{FeatureScope.Name}:{FeatureName}");
        Group<IdentityEndpointGroup>();
        Version(1);
    }

    public override async Task HandleAsync(BanUserRequest req, CancellationToken ct)
    {
        var command = new BanUser.Command(req.Username);

        _ = await _sender.Send(command, ct);

        await SendOkAsync(ct);
    }
}