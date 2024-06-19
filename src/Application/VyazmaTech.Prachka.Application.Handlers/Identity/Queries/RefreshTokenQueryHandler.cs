using VyazmaTech.Prachka.Application.Abstractions.Identity;
using VyazmaTech.Prachka.Application.Abstractions.Identity.Models;
using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.Mapping;
using static VyazmaTech.Prachka.Application.Contracts.Identity.Queries.RefreshToken;

namespace VyazmaTech.Prachka.Application.Handlers.Identity.Queries;

internal sealed class RefreshTokenQueryHandler : IQueryHandler<Query, Response>
{
    private readonly IAuthenticationService _service;

    public RefreshTokenQueryHandler(IAuthenticationService service)
    {
        _service = service;
    }

    public async ValueTask<Response> Handle(Query request, CancellationToken cancellationToken)
    {
        IdentityTokenModel tokens = await _service
            .RefreshToken(request.AccessToken, request.RefreshToken);

        return new Response(tokens.ToDto());
    }
}