using FluentValidation;
using VyazmaTech.Prachka.Application.Core.Errors;
using VyazmaTech.Prachka.Application.Core.Extensions;
using static VyazmaTech.Prachka.Application.Contracts.Orders.Commands.MarkOrderAsPaid;

namespace VyazmaTech.Prachka.Application.Handlers.Order.Commands.MarkOrderAsPaid;

public class MarkOrderAsPaidCommandValidator : AbstractValidator<Command>
{
    public MarkOrderAsPaidCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithError(ValidationErrors.MarkOrderAsPaid.OrderIdIsRequired);
    }
}