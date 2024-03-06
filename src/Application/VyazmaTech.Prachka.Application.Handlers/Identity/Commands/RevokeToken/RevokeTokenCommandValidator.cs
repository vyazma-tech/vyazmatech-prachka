using FluentValidation;
using VyazmaTech.Prachka.Application.Core.Errors;
using VyazmaTech.Prachka.Application.Core.Extensions;
using static VyazmaTech.Prachka.Application.Contracts.Identity.Commands.RevokeToken;

namespace VyazmaTech.Prachka.Application.Handlers.Identity.Commands.RevokeToken;

public sealed class RevokeTokenCommandValidator : AbstractValidator<Command>
{
    public RevokeTokenCommandValidator()
    {
        RuleFor(x => x.TelegramUsername)
            .NotNull()
            .NotEmpty()
            .WithError(ValidationErrors.RevokeToken.UsernameIsRequired);
    }
}