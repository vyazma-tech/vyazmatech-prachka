using Application.Core.Contracts.Common;
using Application.DataAccess.Contracts;
using Application.Dto.Identity;
using Domain.Common.Result;
using static Application.Core.Contracts.Identity.Queries.RefreshToken;

namespace Application.Handlers.Identity.Queries;

internal sealed class RefreshTokenQueryHandler : IQueryHandler<Query, Result<Response>>
{
    private readonly IAuthenticationService _service;

    public RefreshTokenQueryHandler(IAuthenticationService service)
    {
        _service = service;
    }

    public async ValueTask<Result<Response>> Handle(Query request, CancellationToken cancellationToken)
    {
        Result<IdentityTokenDto> result = await _service
            .RefreshToken(request.AccessToken, request.RefreshToken);

        if (result.IsFaulted)
            return new Result<Response>(result.Error);

        return new Response(result.Value);
    }
}