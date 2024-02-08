using Application.Core.Contracts.Common;
using Application.DataAccess.Contracts;
using Domain.Common.Result;
using static Application.Core.Contracts.Identity.Commands.RevokeToken;

namespace Application.Handlers.Identity.Commands.RevokeToken;

internal sealed class RevokeTokenCommandHandler : ICommandHandler<Command, Response>
{
    private readonly IAuthenticationService _service;

    public RevokeTokenCommandHandler(IAuthenticationService service)
    {
        _service = service;
    }

    public async ValueTask<Response> Handle(Command request, CancellationToken cancellationToken)
    {
        Result result = await _service.RevokeToken(request.TelegramUsername, cancellationToken);

        return new Response(result);
    }
}