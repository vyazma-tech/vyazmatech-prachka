using VyazmaTech.Prachka.Application.Abstractions.Identity;
using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Domain.Common.Result;
using static VyazmaTech.Prachka.Application.Contracts.Identity.Commands.RevokeToken;

namespace VyazmaTech.Prachka.Application.Handlers.Identity.Commands.RevokeToken;

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