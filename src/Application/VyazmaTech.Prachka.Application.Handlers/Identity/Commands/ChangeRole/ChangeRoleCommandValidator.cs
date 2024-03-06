using FluentValidation;
using VyazmaTech.Prachka.Application.Core.Errors;
using VyazmaTech.Prachka.Application.Core.Extensions;
using static VyazmaTech.Prachka.Application.Contracts.Identity.Commands.ChangeRole;

namespace VyazmaTech.Prachka.Application.Handlers.Identity.Commands.ChangeRole;

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