using System.Security.Claims;
using Application.Core.Contracts.Common;
using Application.DataAccess.Contracts;
using static Application.Core.Contracts.Identity.Queries.ValidateToken;

namespace Application.Handlers.Identity.Queries;

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

        return ValueTask.FromResult(new Response(result));
    }
}