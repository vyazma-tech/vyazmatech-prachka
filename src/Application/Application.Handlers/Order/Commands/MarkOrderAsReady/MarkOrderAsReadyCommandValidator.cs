using Application.Core.Errors;
using Application.Core.Extensions;
using FluentValidation;
using static Application.Core.Contracts.Orders.Commands.MarkOrderAsReady;

namespace Application.Handlers.Order.Commands.MarkOrderAsReady;

public class MarkOrderAsReadyCommandValidator : AbstractValidator<Command>
{
    public MarkOrderAsReadyCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithError(ValidationErrors.MarkOrderAsReady.OrderIdIsRequired);
    }
}