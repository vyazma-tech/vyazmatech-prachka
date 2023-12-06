using Application.Core.Errors;
using Application.Core.Extensions;
using FluentValidation;

namespace Application.Handlers.Order.Commands.MarkOrderAsReady;

public class MarkOrderAsReadyCommandValidator : AbstractValidator<MarkOrderAsReadyCommand>
{
    public MarkOrderAsReadyCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty()
            .WithError(ValidationErrors.MarkOrderAsReady.OrderIdIsRequired);
    }
}