using FluentValidation;
using VyazmaTech.Prachka.Application.Core.Errors;
using VyazmaTech.Prachka.Application.Core.Extensions;
using static VyazmaTech.Prachka.Application.Contracts.Identity.Commands.RegisterUser;

namespace VyazmaTech.Prachka.Application.Handlers.Identity.Commands.RegisterUser;

public sealed class RegisterUserCommandValidator : AbstractValidator<Command>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(user => user.Credentials)
            .NotNull()
            .WithError(ValidationErrors.CreateUser.InvalidCredentialsFormat);

        RuleFor(user => user.Role)
            .NotNull()
            .NotEmpty()
            .WithError(ValidationErrors.CreateUser.InvalidRoleFormat);
    }
}