using VyazmaTech.Prachka.Application.Abstractions.Identity;
using VyazmaTech.Prachka.Application.Abstractions.Identity.Models;
using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.Mapping;
using static VyazmaTech.Prachka.Application.Contracts.Identity.Commands.RegisterUser;

namespace VyazmaTech.Prachka.Application.Handlers.Identity.Commands.RegisterUser;

internal sealed class RegisterUserCommandHandler : ICommandHandler<Command, Response>
{
    private readonly IAuthenticationService _service;

    public RegisterUserCommandHandler(IAuthenticationService service)
    {
        _service = service;
    }

    public async ValueTask<Response> Handle(Command request, CancellationToken cancellationToken)
    {
        IdentityUserModel user = await _service
            .CreateUserAsync(request.Id, request.Credentials, request.Role, cancellationToken);

        IdentityTokenModel tokens = await _service.GetUserTokensAsync(user.TelegramUsername, cancellationToken);
        string role = await _service.GetUserRoleAsync(user.TelegramUsername, cancellationToken);

        return new Response(user.ToDto(), tokens.ToDto(role));
    }
}