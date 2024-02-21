using FastEndpoints;
using Mediator;
using VyazmaTech.Prachka.Application.Contracts.Identity.Commands;
using VyazmaTech.Prachka.Domain.Common.Result;
using VyazmaTech.Prachka.Presentation.Endpoints.Identity.Models;

namespace VyazmaTech.Prachka.Presentation.Endpoints.Identity;

internal class ChangeRoleEndpoint : Endpoint<ChangeRoleRequest, Result>
{
    private readonly ISender _sender;

    public ChangeRoleEndpoint(ISender sender)
    {
        _sender = sender;
    }

    public override void Configure()
    {
        Post("api/change");
        AllowAnonymous();
    }

    public override async Task HandleAsync(ChangeRoleRequest req, CancellationToken ct)
    {
        var command = new ChangeRole.Command(req.Username, req.NewRoleName);

        ChangeRole.Response response = await _sender.Send(command, ct);

        // TODO: handle edge case
        await SendOkAsync(response.Result, ct);
    }
}