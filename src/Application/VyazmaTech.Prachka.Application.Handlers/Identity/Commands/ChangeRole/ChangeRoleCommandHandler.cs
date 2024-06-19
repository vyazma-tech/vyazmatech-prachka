using VyazmaTech.Prachka.Application.Abstractions.Identity;
using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Domain.Common.Exceptions;
using VyazmaTech.Prachka.Infrastructure.Authentication.Errors;
using static VyazmaTech.Prachka.Application.Contracts.Identity.Commands.ChangeRole;

namespace VyazmaTech.Prachka.Application.Handlers.Identity.Commands.ChangeRole;

internal sealed class ChangeRoleCommandHandler : ICommandHandler<Command, Response>
{
    private readonly IAuthenticationService _service;
    private readonly ICurrentUser _currentUser;

    public ChangeRoleCommandHandler(IAuthenticationService service, ICurrentUser currentUser)
    {
        _service = service;
        _currentUser = currentUser;
    }

    public async ValueTask<Response> Handle(Command request, CancellationToken cancellationToken)
    {
        string role = await _service
            .GetUserRoleAsync(request.TelegramUsername, cancellationToken);

        if (!_currentUser.CanChangeUserRole(role, request.NewRoleName))
            throw new IdentityException(AuthenticationErrors.IdentityUser.NotInRole());

        await _service.UpdateUserRoleAsync(request.TelegramUsername, request.NewRoleName, cancellationToken);

        return default;
    }
}