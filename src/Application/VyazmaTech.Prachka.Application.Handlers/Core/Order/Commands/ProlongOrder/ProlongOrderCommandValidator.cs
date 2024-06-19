using FluentValidation;
using VyazmaTech.Prachka.Application.Core.Errors;
using VyazmaTech.Prachka.Application.Core.Extensions;

namespace VyazmaTech.Prachka.Application.Handlers.Core.Order.Commands.ProlongOrder;

public class ProlongOrderCommandValidator : AbstractValidator<Contracts.Core.Orders.Commands.ProlongOrder.Command>
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