using FastEndpoints;
using Mediator;
using VyazmaTech.Prachka.Application.Contracts.Identity.Queries;
using VyazmaTech.Prachka.Application.Dto.Identity;
using VyazmaTech.Prachka.Presentation.Endpoints.Identity.V1.Models;

namespace VyazmaTech.Prachka.Presentation.Endpoints.Identity.V1;

internal class ValidateTokenEndpoint : Endpoint<ValidateTokenRequest, PrincipalDto>
{
    private readonly ISender _sender;

    public ValidateTokenEndpoint(ISender sender)
    {
        _sender = sender;
    }

    public override void Configure()
    {
        Put("validate");
        AllowAnonymous();
        Group<IdentityEndpointGroup>();
        Version(1);
    }

    public override async Task HandleAsync(ValidateTokenRequest req, CancellationToken ct)
    {
        var query = new ValidateToken.Query(req.AccessToken);

        ValidateToken.Response response = await _sender.Send(query, ct);

        await SendOkAsync(response.Principal, ct);
    }
}