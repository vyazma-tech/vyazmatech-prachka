using FluentValidation;
using VyazmaTech.Prachka.Application.Contracts.Core.Users.Commands;
using VyazmaTech.Prachka.Application.Core.Errors;
using VyazmaTech.Prachka.Application.Core.Extensions;

namespace VyazmaTech.Prachka.Application.Handlers.Core.User.Commands;

internal sealed class CreateUserCommandValidator : AbstractValidator<CreateUser.Command>
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