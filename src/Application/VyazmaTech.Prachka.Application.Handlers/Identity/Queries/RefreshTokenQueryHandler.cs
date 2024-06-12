using VyazmaTech.Prachka.Application.Abstractions.Identity;
using VyazmaTech.Prachka.Application.Abstractions.Identity.Models;
using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.Mapping;
using VyazmaTech.Prachka.Domain.Common.Result;
using static VyazmaTech.Prachka.Application.Contracts.Identity.Queries.RefreshToken;

namespace VyazmaTech.Prachka.Application.Handlers.Identity.Queries;

internal sealed class RefreshTokenQueryHandler : IQueryHandler<Query, Result<Response>>
{
    private readonly IAuthenticationService _service;

    public RefreshTokenQueryHandler(IAuthenticationService service)
    {
        _service = service;
    }

    public async ValueTask<Result<Response>> Handle(Query request, CancellationToken cancellationToken)
    {
        Result<IdentityTokenModel> result = await _service
            .RefreshToken(request.AccessToken, request.RefreshToken);

        if (result.IsFaulted)
        {
            return new Result<Response>(result.Error);
        }

        IdentityTokenModel tokens = result.Value;

        return new Response(tokens.ToDto());
    }
}