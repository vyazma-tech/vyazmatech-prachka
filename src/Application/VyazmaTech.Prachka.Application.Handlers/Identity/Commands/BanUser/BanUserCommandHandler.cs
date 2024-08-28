using Microsoft.Extensions.Logging;
using VyazmaTech.Prachka.Application.Abstractions.Identity;
using VyazmaTech.Prachka.Application.Abstractions.Identity.Models;
using VyazmaTech.Prachka.Application.Contracts.Common;
using static VyazmaTech.Prachka.Application.Contracts.Identity.Commands.BanUser;

namespace VyazmaTech.Prachka.Application.Handlers.Identity.Commands.BanUser;

internal sealed class BanUserCommandHandler : ICommandHandler<Command, Response>
{
    private readonly ICurrentUser _currentUser;
    private readonly IAuthenticationService _service;
    private readonly IBanUserService _banService;
    private readonly ILogger<BanUserCommandHandler> _logger;

    public BanUserCommandHandler(
        ICurrentUser currentUser,
        IAuthenticationService service,
        IBanUserService banService,
        ILogger<BanUserCommandHandler> logger)
    {
        _currentUser = currentUser;
        _service = service;
        _banService = banService;
        _logger = logger;
    }

    public async ValueTask<Response> Handle(Command request, CancellationToken cancellationToken)
    {
        Guid? currentUserId = _currentUser.Id;

        try
        {
            IdentityUserModel currentUser = await _service.GetUserByIdAsync(currentUserId!.Value, cancellationToken);

            await _banService.BanUser(request.Username, currentUser.TelegramUsername);
        }
        catch (Exception e)
        {
            _logger.LogWarning(e, "Ban user threw an exception");
        }

        return default;
    }
}