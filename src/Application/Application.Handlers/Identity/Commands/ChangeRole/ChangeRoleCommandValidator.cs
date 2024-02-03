using Application.Core.Errors;
using Application.Core.Extensions;
using FluentValidation;
using static Application.Core.Contracts.Identity.Commands.ChangeRole;

namespace Application.Handlers.Identity.Commands.ChangeRole;

public sealed class ChangeRoleCommandValidator : AbstractValidator<Command>
{
    public ChangeRoleCommandValidator()
    {
        RuleFor(x => x.TelegramUsername)
            .NotNull()
            .NotEmpty()
            .WithError(ValidationErrors.ChangeRole.UsernameIsRequired);

        RuleFor(x => x.NewRoleName)
            .NotNull()
            .NotEmpty()
            .WithError(ValidationErrors.ChangeRole.NewRoleNameIsRequired);
    }
}