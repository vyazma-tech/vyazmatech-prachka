using FluentValidation;
using VyazmaTech.Prachka.Application.Core.Errors;
using VyazmaTech.Prachka.Application.Core.Extensions;
using static VyazmaTech.Prachka.Application.Contracts.Users.Commands.CreateUser;

namespace VyazmaTech.Prachka.Application.Handlers.User.Commands;

internal sealed class CreateUserCommandValidator : AbstractValidator<Command>
{
    public CreateUserCommandValidator()
    {
        RuleFor(user => user.Id)
            .NotEmpty()
            .WithError(ValidationErrors.CreateUser.UserIdIsRequired);

        RuleFor(user => user.TelegramUsername)
            .NotEmpty()
            .WithError(ValidationErrors.CreateUser.InvalidCredentialsFormat);

        RuleFor(user => user.Fullname)
            .NotEmpty()
            .WithError(ValidationErrors.CreateUser.InvalidCredentialsFormat);
    }
}