using FastEndpoints;
using Mediator;
using VyazmaTech.Prachka.Application.Contracts.Identity.Commands;
using VyazmaTech.Prachka.Presentation.Authorization;
using VyazmaTech.Prachka.Presentation.Endpoints.Identity.V1.Models;

namespace VyazmaTech.Prachka.Presentation.Endpoints.Identity.V1;

internal sealed class UnbanUserEndpoint : Endpoint<UnbanUserRequest>
{
    private const string FeatureName = "UnbanUser";
    private readonly ISender _sender;

    public UnbanUserEndpoint(ISender sender)
    {
        _sender = sender;
    }

    public override void Configure()
    {
        Post("unban");
        Policies($"{AuthorizeFeatureAttribute.Prefix}{FeatureScope.Name}:{FeatureName}");
        Group<IdentityEndpointGroup>();
        Version(1);
    }

    public override async Task HandleAsync(UnbanUserRequest req, CancellationToken ct)
    {
        var command = new UnbanUser.Command(req.Username);

        _ = await _sender.Send(command, ct);

        await SendOkAsync(ct);
    }
}