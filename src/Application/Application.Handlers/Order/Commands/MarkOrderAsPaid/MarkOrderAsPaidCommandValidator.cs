using Application.Core.Errors;
using Application.Core.Extensions;
using FluentValidation;
using static Application.Core.Contracts.Orders.Commands.MarkOrderAsPaid;

namespace Application.Handlers.Order.Commands.MarkOrderAsPaid;

public class MarkOrderAsPaidCommandValidator : AbstractValidator<Command>
{
    public MarkOrderAsPaidCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithError(ValidationErrors.MarkOrderAsPaid.OrderIdIsRequired);
    }
}