using FluentValidation;
using VyazmaTech.Prachka.Application.Core.Errors;
using VyazmaTech.Prachka.Application.Core.Extensions;
using static VyazmaTech.Prachka.Application.Contracts.Identity.Commands.BanUser;

namespace VyazmaTech.Prachka.Application.Handlers.Identity.Commands.BanUser;

public sealed class BanUserCommandValidator : AbstractValidator<Command>
{
    public BanUserCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotNull()
            .NotEmpty()
            .Matches("^@[a-z-A-Z-0-9]*$")
            .WithError(ValidationErrors.BanUser.InvalidUsername);
    }
}