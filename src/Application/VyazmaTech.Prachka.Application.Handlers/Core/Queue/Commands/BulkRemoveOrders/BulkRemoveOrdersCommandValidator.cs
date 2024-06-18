using FluentValidation;
using VyazmaTech.Prachka.Application.Core.Errors;
using VyazmaTech.Prachka.Application.Core.Extensions;

namespace VyazmaTech.Prachka.Application.Handlers.Core.Queue.Commands.BulkRemoveOrders;

public sealed class
    BulkRemoveOrdersCommandValidator : AbstractValidator<Contracts.Core.Queues.Commands.BulkRemoveOrders.Command>
{
    public BulkRemoveOrdersCommandValidator()
    {
        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithError(ValidationErrors.BulkRemoveOrders.OrderQuantityShouldBePositive);
    }
}