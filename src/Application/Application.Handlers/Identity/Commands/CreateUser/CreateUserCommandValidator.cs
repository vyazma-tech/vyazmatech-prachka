using Application.Core.Errors;
using Application.Core.Extensions;
using FluentValidation;
using static Application.Core.Contracts.Identity.Commands.CreateUser;

namespace Application.Handlers.Identity.Commands.CreateUser;

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