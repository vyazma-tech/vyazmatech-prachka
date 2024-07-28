using FluentValidation;
using VyazmaTech.Prachka.Application.Core.Errors;
using VyazmaTech.Prachka.Application.Core.Extensions;

namespace VyazmaTech.Prachka.Application.Handlers.Core.Order.Commands.MarkOrderAsPaid;

public class MarkOrderAsPaidCommandValidator : AbstractValidator<Contracts.Core.Orders.Commands.MarkOrderAsPaid.Command>
{
    public MarkOrderAsPaidCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithError(ValidationErrors.MarkOrderAsPaid.OrderIdIsRequired);

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0)
            .WithError(ValidationErrors.MarkOrderAsPaid.NegativePrice);
    }
}