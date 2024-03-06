using FastEndpoints;
using Mediator;
using VyazmaTech.Prachka.Application.Contracts.Identity.Queries;
using VyazmaTech.Prachka.Application.Dto.Identity;
using VyazmaTech.Prachka.Domain.Common.Result;
using VyazmaTech.Prachka.Presentation.Endpoints.Extensions;
using VyazmaTech.Prachka.Presentation.Endpoints.Identity.V1.Models;

namespace VyazmaTech.Prachka.Presentation.Endpoints.Identity.V1;

internal class RefreshTokenEndpoint : Endpoint<RefreshTokenRequest, IdentityTokenDto>
{
    private readonly ISender _sender;

    public RefreshTokenEndpoint(ISender sender)
    {
        _sender = sender;
    }

    public override void Configure()
    {
        Put("refresh");
        AllowAnonymous();
        Group<IdentityEndpointGroup>();
        Version(1);
    }

    public override async Task HandleAsync(RefreshTokenRequest req, CancellationToken ct)
    {
        var query = new RefreshToken.Query(req.AccessToken, req.RefreshToken);

        Result<RefreshToken.Response> response = await _sender.Send(query, ct);

        await response.Match(
            success => SendOkAsync(success.Tokens, ct),
            _ => this.SendProblemsAsync(response.ToProblemDetails()));
    }
}