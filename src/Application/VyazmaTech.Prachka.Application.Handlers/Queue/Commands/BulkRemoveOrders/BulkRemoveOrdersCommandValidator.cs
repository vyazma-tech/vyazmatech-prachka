using FluentValidation;
using VyazmaTech.Prachka.Application.Core.Errors;
using VyazmaTech.Prachka.Application.Core.Extensions;
using static VyazmaTech.Prachka.Application.Contracts.Queues.Commands.BulkRemoveOrders;

namespace VyazmaTech.Prachka.Application.Handlers.Queue.Commands.BulkRemoveOrders;

public sealed class BulkRemoveOrdersCommandValidator : AbstractValidator<Command>
{
    public BulkRemoveOrdersCommandValidator()
    {
        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithError(ValidationErrors.BulkRemoveOrders.OrderQuantityShouldBePositive);
    }
}