using FluentValidation;
using VyazmaTech.Prachka.Application.Core.Errors;
using VyazmaTech.Prachka.Application.Core.Extensions;

namespace VyazmaTech.Prachka.Application.Handlers.Core.Order.Commands.MarkOrderAsReady;

public class
    MarkOrderAsReadyCommandValidator : AbstractValidator<Contracts.Core.Orders.Commands.MarkOrderAsReady.Command>
{
    public MarkOrderAsReadyCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithError(ValidationErrors.MarkOrderAsReady.OrderIdIsRequired);
    }
}