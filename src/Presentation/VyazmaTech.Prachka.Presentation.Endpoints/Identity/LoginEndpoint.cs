using FastEndpoints;
using Mediator;
using VyazmaTech.Prachka.Application.Contracts.Identity.Queries;
using VyazmaTech.Prachka.Application.Dto.Identity;
using VyazmaTech.Prachka.Domain.Common.Result;
using VyazmaTech.Prachka.Presentation.Endpoints.Extensions;
using VyazmaTech.Prachka.Presentation.Endpoints.Identity.Models;

namespace VyazmaTech.Prachka.Presentation.Endpoints.Identity;

internal class LoginEndpoint : Endpoint<LoginRequest, IdentityTokenDto>
{
    private readonly ISender _sender;

    public LoginEndpoint(ISender sender)
    {
        _sender = sender;
    }

    public override void Configure()
    {
        Put("api/identity/login");
        AllowAnonymous();
    }

    public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
    {
        var query = new Login.Query(req.Username);

        Result<Login.Response> response = await _sender.Send(query, ct);

        await response.Match(
            success => SendOkAsync(success.Tokens, ct),
            _ => this.SendProblemsAsync(response.ToProblemDetails()));
    }
}