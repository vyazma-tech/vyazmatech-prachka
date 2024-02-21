using FastEndpoints;
using Mediator;
using VyazmaTech.Prachka.Application.Contracts.Identity.Queries;
using VyazmaTech.Prachka.Application.Dto.Identity;
using VyazmaTech.Prachka.Presentation.Endpoints.Identity.Models;

namespace VyazmaTech.Prachka.Presentation.Endpoints.Identity;

internal class ValidateTokenEndpoint : Endpoint<ValidateTokenRequest, PrincipalDto>
{
    private readonly ISender _sender;

    public ValidateTokenEndpoint(ISender sender)
    {
        _sender = sender;
    }

    public override void Configure()
    {
        Put("api/validate");
        AllowAnonymous();
    }

    public override async Task HandleAsync(ValidateTokenRequest req, CancellationToken ct)
    {
        var query = new ValidateToken.Query(req.AccessToken);

        ValidateToken.Response response = await _sender.Send(query, ct);

        // TODO: handle edge case
        await SendOkAsync(response.Principal, ct);
    }
}