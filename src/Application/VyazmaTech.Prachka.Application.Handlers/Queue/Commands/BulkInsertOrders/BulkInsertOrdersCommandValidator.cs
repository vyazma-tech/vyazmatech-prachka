using FluentValidation;
using VyazmaTech.Prachka.Application.Core.Errors;
using VyazmaTech.Prachka.Application.Core.Extensions;
using static VyazmaTech.Prachka.Application.Contracts.Queues.Commands.BulkInsertOrders;

namespace VyazmaTech.Prachka.Application.Handlers.Queue.Commands.BulkInsertOrders;

public sealed class BulkInsertOrdersCommandValidator : AbstractValidator<Command>
{
    public BulkInsertOrdersCommandValidator()
    {
        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithError(ValidationErrors.BulkInsertOrders.OrderQuantityShouldBePositive);
    }
}