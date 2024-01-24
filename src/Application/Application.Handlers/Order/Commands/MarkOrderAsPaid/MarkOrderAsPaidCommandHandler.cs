﻿using Application.Core.Contracts.Common;
using Application.Core.Specifications;
using Application.DataAccess.Contracts;
using Domain.Common.Result;
using Domain.Core.Order;
using Domain.Kernel;
using static Application.Core.Contracts.Orders.Commands.MarkOrderAsPaid;

namespace Application.Handlers.Order.Commands.MarkOrderAsPaid;

internal sealed class MarkOrderAsPaidCommandHandler : ICommandHandler<Command, Result<Response>>
{
    private readonly IPersistenceContext _persistenceContext;
    private readonly IDateTimeProvider _dateTimeProvider;

    public MarkOrderAsPaidCommandHandler(
        IDateTimeProvider dateTimeProvider,
        IPersistenceContext persistenceContext)
    {
        _persistenceContext = persistenceContext;
        _dateTimeProvider = dateTimeProvider;
    }

    public async ValueTask<Result<Response>> Handle(Command request, CancellationToken cancellationToken)
    {
        Result<OrderEntity> searchResult = await _persistenceContext.Orders
            .FindByIdAsync(request.Id, cancellationToken);

        if (searchResult.IsFaulted)
            return new Result<Response>(searchResult.Error);

        OrderEntity order = searchResult.Value;

        Result<OrderEntity> makePaidResult = order.MakePayment(_dateTimeProvider.SpbDateTimeNow);

        if (makePaidResult.IsFaulted)
            return new Result<Response>(makePaidResult.Error);

        order = makePaidResult.Value;

        _persistenceContext.Orders.Update(order);
        await _persistenceContext.SaveChangesAsync(cancellationToken);

        return order.ToDto();
    }
}