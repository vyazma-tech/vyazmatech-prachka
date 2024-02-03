using Application.Core.Abstractions;
using Application.Core.Contracts.Common;
using Application.DataAccess.Contracts;
using Domain.Common.Result;
using static Application.Core.Contracts.Identity.Commands.ChangeRole;

namespace Application.Handlers.Identity.Commands.ChangeRole;

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
        Result<string> searchResult = await _service
            .GetUserRoleAsync(request.TelegramUsername, cancellationToken);

        if (searchResult.IsFaulted)
            return new Response(searchResult);

        string role = searchResult.Value;

        if (!_currentUser.CanChangeUserRole(role, request.NewRoleName))
            return new Response(Result.Failure());

        await _service.UpdateUserRoleAsync(request.TelegramUsername, request.NewRoleName, cancellationToken);

        return new Response(Result.Success());
    }
}