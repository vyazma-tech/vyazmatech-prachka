using VyazmaTech.Prachka.Application.Abstractions.Identity;
using VyazmaTech.Prachka.Application.Contracts.Common;
using static VyazmaTech.Prachka.Application.Contracts.Identity.Commands.UnbanUser;

namespace VyazmaTech.Prachka.Application.Handlers.Identity.Commands.UnbanUser;

internal sealed class UnbanUserCommandHandler : ICommandHandler<Command, Response>
{
    private readonly IBanUserService _banService;

    public UnbanUserCommandHandler(IBanUserService banService)
    {
        _banService = banService;
    }

    public async ValueTask<Response> Handle(Command request, CancellationToken cancellationToken)
    {
        await _banService.UnbanUser(request.Username);

        return default;
    }
}