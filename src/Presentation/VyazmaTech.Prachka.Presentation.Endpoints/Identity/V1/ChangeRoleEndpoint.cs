using FastEndpoints;
using Mediator;
using VyazmaTech.Prachka.Application.Contracts.Identity.Commands;
using VyazmaTech.Prachka.Domain.Common.Exceptions;
using VyazmaTech.Prachka.Presentation.Authorization;
using VyazmaTech.Prachka.Presentation.Endpoints.Extensions;
using VyazmaTech.Prachka.Presentation.Endpoints.Identity.V1.Models;

namespace VyazmaTech.Prachka.Presentation.Endpoints.Identity.V1;

internal class ChangeRoleEndpoint : Endpoint<ChangeRoleRequest, ChangeRole.Response>
{
    private const string FeatureName = "ChangeRole";
    private readonly ISender _sender;

    public ChangeRoleEndpoint(ISender sender)
    {
        _sender = sender;
    }

    public override void Configure()
    {
        Post("change-role");
        Policies($"{AuthorizeFeatureAttribute.Prefix}{FeatureScope.Name}:{FeatureName}");
        Group<IdentityEndpointGroup>();
        Version(1);
    }

    public override async Task HandleAsync(ChangeRoleRequest req, CancellationToken ct)
    {
        var command = new ChangeRole.Command(req.Username, req.NewRoleName);

        try
        {
            ChangeRole.Response response = await _sender.Send(command, ct);

            await SendOkAsync(response, ct);
        }
        catch (IdentityException e)
        {
            await this.SendProblemsAsync(e.ToProblemDetails());
        }
    }
}