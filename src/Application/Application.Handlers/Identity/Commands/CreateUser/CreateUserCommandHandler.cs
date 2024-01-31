using Application.Core.Contracts.Common;
using Application.DataAccess.Contracts;
using Application.Dto.Identity;
using Domain.Common.Result;
using static Application.Core.Contracts.Identity.Commands.CreateUser;

namespace Application.Handlers.Identity.Commands.CreateUser;

internal sealed class CreateUserCommandHandler : ICommandHandler<Command, Result<Response>>
{
    private readonly IAuthenticationService _service;

    public CreateUserCommandHandler(IAuthenticationService service)
    {
        _service = service;
    }

    public async ValueTask<Result<Response>> Handle(Command request, CancellationToken cancellationToken)
    {
        Result<IdentityUserDto> creationResult = await _service
            .CreateUserAsync(request.Id, request.Credentials, request.Role, cancellationToken);

        if (creationResult.IsFaulted)
            return new Result<Response>(creationResult.Error);

        IdentityUserDto user = creationResult.Value;

        Result<IdentityTokenDto> tokens = await _service.GetUserTokensAsync(user.TelegramUsername, cancellationToken);

        return new Response(user, tokens.Value);
    }
}