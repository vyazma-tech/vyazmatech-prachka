using VyazmaTech.Prachka.Application.Abstractions.Identity;
using VyazmaTech.Prachka.Application.Abstractions.Identity.Models;
using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.Mapping;
using VyazmaTech.Prachka.Domain.Common.Result;
using static VyazmaTech.Prachka.Application.Contracts.Identity.Queries.Login;

namespace VyazmaTech.Prachka.Application.Handlers.Identity.Queries;

internal sealed class LoginQueryHandler : IQueryHandler<Query, Result<Response>>
{
    private readonly IAuthenticationService _service;

    public LoginQueryHandler(IAuthenticationService service)
    {
        _service = service;
    }

    public async ValueTask<Result<Response>> Handle(Query request, CancellationToken cancellationToken)
    {
        Result<IdentityTokenModel> loginResult = await _service
            .GetUserTokensAsync(request.Username, cancellationToken);

        if (loginResult.IsFaulted)
        {
            return new Result<Response>(loginResult.Error);
        }

        IdentityTokenModel tokens = loginResult.Value;

        return new Response(tokens.ToDto());
    }
}