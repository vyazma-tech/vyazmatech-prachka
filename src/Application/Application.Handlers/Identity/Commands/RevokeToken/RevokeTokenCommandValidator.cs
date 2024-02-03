using Application.Core.Errors;
using Application.Core.Extensions;
using FluentValidation;
using static Application.Core.Contracts.Identity.Commands.RevokeToken;

namespace Application.Handlers.Identity.Commands.RevokeToken;

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