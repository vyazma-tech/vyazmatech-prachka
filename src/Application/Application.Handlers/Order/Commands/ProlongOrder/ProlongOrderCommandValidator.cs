using Application.Core.Errors;
using Application.Core.Extensions;
using FluentValidation;
using static Application.Core.Contracts.Orders.Commands.ProlongOrder;

namespace Application.Handlers.Order.Commands.ProlongOrder;

public class ProlongOrderCommandValidator : AbstractValidator<Command>
{
    public ProlongOrderCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty()
            .WithError(ValidationErrors.ProlongOrder.OrderIdIsRequired);

        RuleFor(x => x.TargetQueueId)
            .NotEmpty()
            .WithError(ValidationErrors.ProlongOrder.TargetQueueIdIdIsRequired);
    }
}