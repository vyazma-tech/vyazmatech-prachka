using FluentValidation;
using VyazmaTech.Prachka.Application.Core.Errors;
using VyazmaTech.Prachka.Application.Core.Extensions;
using static VyazmaTech.Prachka.Application.Contracts.Orders.Commands.MarkOrderAsReady;

namespace VyazmaTech.Prachka.Application.Handlers.Order.Commands.MarkOrderAsReady;

public class MarkOrderAsReadyCommandValidator : AbstractValidator<Command>
{
    public MarkOrderAsReadyCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithError(ValidationErrors.MarkOrderAsReady.OrderIdIsRequired);
    }
}