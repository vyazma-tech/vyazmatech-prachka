using Application.Core.Errors;
using Application.Core.Extensions;
using FluentValidation;

namespace Application.Handlers.Order.Commands.MarkOrderAsPaid;

public class MarkOrderAsPaidCommandValidator : AbstractValidator<MarkOrderAsPaidCommand>
{
    public MarkOrderAsPaidCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty()
            .WithError(ValidationErrors.MarkOrderAsPaid.OrderIdIsRequired);
    }
}