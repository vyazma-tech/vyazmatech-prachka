using FluentValidation;
using VyazmaTech.Prachka.Application.Core.Errors;
using VyazmaTech.Prachka.Application.Core.Extensions;

namespace VyazmaTech.Prachka.Application.Handlers.Core.Queue.Commands.BulkInsertOrders;

public sealed class
    BulkInsertOrdersCommandValidator : AbstractValidator<Contracts.Core.Queues.Commands.BulkInsertOrders.Command>
{
    public BulkInsertOrdersCommandValidator()
    {
        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithError(ValidationErrors.BulkInsertOrders.OrderQuantityShouldBePositive);
    }
}