using FastEndpoints;
using Mediator;
using VyazmaTech.Prachka.Application.Contracts.Identity.Queries;
using VyazmaTech.Prachka.Application.Dto.Identity;

namespace VyazmaTech.Prachka.Presentation.Endpoints.Identity;

internal class GetAllRolesEndpoint : EndpointWithoutRequest<AllRolesDto>
{
    private readonly ISender _sender;

    public GetAllRolesEndpoint(ISender sender)
    {
        _sender = sender;
    }

    public override void Configure()
    {
        Get("api/roles");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var query = default(GetAllRoles.Query);

        GetAllRoles.Response response = await _sender.Send(query, ct);

        await SendOkAsync(response.Roles, ct);
    }
}