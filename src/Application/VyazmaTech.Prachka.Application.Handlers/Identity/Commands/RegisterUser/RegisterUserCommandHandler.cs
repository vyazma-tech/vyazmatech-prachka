using VyazmaTech.Prachka.Application.Abstractions.Identity;
using VyazmaTech.Prachka.Application.Abstractions.Identity.Models;
using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.Mapping;
using VyazmaTech.Prachka.Domain.Common.Result;
using static VyazmaTech.Prachka.Application.Contracts.Identity.Commands.RegisterUser;

namespace VyazmaTech.Prachka.Application.Handlers.Identity.Commands.RegisterUser;

internal sealed class RegisterUserCommandHandler : ICommandHandler<Command, Result<Response>>
{
    private readonly IAuthenticationService _service;

    public RegisterUserCommandHandler(IAuthenticationService service)
    {
        _service = service;
    }

    public async ValueTask<Result<Response>> Handle(Command request, CancellationToken cancellationToken)
    {
        Result<IdentityUserModel> creationResult = await _service
            .CreateUserAsync(request.Id, request.Credentials, request.Role, cancellationToken);

        if (creationResult.IsFaulted)
            return new Result<Response>(creationResult.Error);

        IdentityUserModel user = creationResult.Value;

        Result<IdentityTokenModel> tokens = await _service.GetUserTokensAsync(user.TelegramUsername, cancellationToken);
        Result<string> role = await _service.GetUserRoleAsync(user.TelegramUsername, cancellationToken);

        return new Response(user.ToDto(role.Value), tokens.Value.ToDto());
    }
}