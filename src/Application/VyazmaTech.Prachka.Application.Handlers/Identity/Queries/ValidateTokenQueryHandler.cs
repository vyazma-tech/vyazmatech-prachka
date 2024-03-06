using System.Security.Claims;
using VyazmaTech.Prachka.Application.Abstractions.Identity;
using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.Dto.Identity;
using static VyazmaTech.Prachka.Application.Contracts.Identity.Queries.ValidateToken;

namespace VyazmaTech.Prachka.Application.Handlers.Identity.Queries;

internal sealed class ValidateTokenQueryHandler : IQueryHandler<Query, Response>
{
    private readonly IAuthenticationService _service;

    public ValidateTokenQueryHandler(IAuthenticationService service)
    {
        _service = service;
    }

    public ValueTask<Response> Handle(Query request, CancellationToken cancellationToken)
    {
        ClaimsPrincipal? result = _service.DecodePrincipal(request.AccessToken);

        var dto = new PrincipalDto(result);

        return ValueTask.FromResult(new Response(dto));
    }
}