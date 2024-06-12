using FastEndpoints;
using Mediator;
using VyazmaTech.Prachka.Application.Contracts.Identity.Queries;
using VyazmaTech.Prachka.Application.Dto.Identity;
using VyazmaTech.Prachka.Domain.Common.Exceptions;
using VyazmaTech.Prachka.Presentation.Endpoints.Extensions;
using VyazmaTech.Prachka.Presentation.Endpoints.Identity.V1.Models;

namespace VyazmaTech.Prachka.Presentation.Endpoints.Identity.V1;

internal class LoginEndpoint : Endpoint<LoginRequest, IdentityTokenDto>
{
    private readonly ISender _sender;

    public LoginEndpoint(ISender sender)
    {
        _sender = sender;
    }

    public override void Configure()
    {
        Put("login");
        AllowAnonymous();
        Group<IdentityEndpointGroup>();
        Version(1);
    }

    public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
    {
        var query = new Login.Query(req.Username);

        try
        {
            Login.Response response = await _sender.Send(query, ct);
            await SendOkAsync(response.Tokens, ct);
        }
        catch (IdentityException e)
        {
            await this.SendProblemsAsync(e.ToProblemDetails());
        }
    }
}