using VyazmaTech.Prachka.Application.Abstractions.Identity;
using VyazmaTech.Prachka.Application.Abstractions.Identity.Models;
using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.Mapping;
using static VyazmaTech.Prachka.Application.Contracts.Identity.Queries.Login;

namespace VyazmaTech.Prachka.Application.Handlers.Identity.Queries;

internal sealed class LoginQueryHandler : IQueryHandler<Query, Response>
{
    private readonly IAuthenticationService _service;

    public LoginQueryHandler(IAuthenticationService service)
    {
        _service = service;
    }

    public async ValueTask<Response> Handle(Query request, CancellationToken cancellationToken)
    {
        IdentityTokenModel tokens = await _service
            .GetUserTokensAsync(request.Username, cancellationToken);

        return new Response(tokens.ToDto());
    }
}