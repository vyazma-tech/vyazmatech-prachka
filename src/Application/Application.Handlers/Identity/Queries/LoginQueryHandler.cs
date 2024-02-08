using Application.Core.Contracts.Common;
using Application.DataAccess.Contracts;
using Application.Dto.Identity;
using Domain.Common.Result;
using static Application.Core.Contracts.Identity.Queries.Login;

namespace Application.Handlers.Identity.Queries;

internal sealed class LoginQueryHandler : IQueryHandler<Query, Result<Response>>
{
    private readonly IAuthenticationService _service;

    public LoginQueryHandler(IAuthenticationService service)
    {
        _service = service;
    }

    public async ValueTask<Result<Response>> Handle(Query request, CancellationToken cancellationToken)
    {
        Result<IdentityTokenDto> loginResult = await _service
            .GetUserTokensAsync(request.Username, cancellationToken);

        if (loginResult.IsFaulted)
            return new Result<Response>(loginResult.Error);

        return new Response(loginResult.Value);
    }
}