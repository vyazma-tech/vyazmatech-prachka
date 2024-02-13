using FluentValidation;
using VyazmaTech.Prachka.Application.Core.Errors;
using VyazmaTech.Prachka.Application.Core.Extensions;
using static VyazmaTech.Prachka.Application.Contracts.Identity.Commands.CreateUser;

namespace VyazmaTech.Prachka.Application.Handlers.Identity.Commands.CreateUser;

public sealed class CreateUserCommandValidator : AbstractValidator<Command>
{
    public CreateUserCommandValidator()
    {
        RuleFor(user => user.Id)
            .NotEmpty()
            .WithError(ValidationErrors.CreateUser.UserIdIsRequired);

        RuleFor(user => user.Credentials)
            .NotNull()
            .WithError(ValidationErrors.CreateUser.InvalidCredentialsFormat);

        RuleFor(user => user.Role)
            .NotNull()
            .NotEmpty()
            .WithError(ValidationErrors.CreateUser.InvalidRoleFormat);
    }
}