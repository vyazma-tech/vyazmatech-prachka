using VyazmaTech.Prachka.Application.Abstractions.Querying.OrderSubscription;
using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.Core.Errors;
using VyazmaTech.Prachka.Application.Core.Specifications;
using VyazmaTech.Prachka.Application.DataAccess.Contracts;
using VyazmaTech.Prachka.Domain.Common.Result;
using VyazmaTech.Prachka.Domain.Core.Order;
using VyazmaTech.Prachka.Domain.Core.Subscription;
using static VyazmaTech.Prachka.Application.Contracts.Subscriptions.SubscribeToOrder;

namespace VyazmaTech.Prachka.Application.Handlers.Subscriptions;

internal sealed class SubscribeToOrderCommandHandler : ICommandHandler<Command, Result<Response>>
{
    private readonly IPersistenceContext _context;

    public SubscribeToOrderCommandHandler(IPersistenceContext context)
    {
        _context = context;
    }

    public async ValueTask<Result<Response>> Handle(Command request, CancellationToken cancellationToken)
    {
        if (request.UserId is null)
        {
            return new Result<Response>(ApplicationErrors.Subscription.AnonymousUserCantSubscribe);
        }

        var query = OrderSubscriptionQuery.Build(x => x.WithUserId(request.UserId.Value));
        OrderSubscriptionEntity? existing = await _context.OrderSubscriptions
            .QueryAsync(query, cancellationToken)
            .FirstOrDefaultAsync(cancellationToken);

        if (existing is null)
        {
            return new Result<Response>(ApplicationErrors.Subscription.UserHasNoSubscriptions(request.UserId.Value));
        }

        Result<OrderEntity> orderSearchResult = await _context.Orders.FindByIdAsync(request.OrderId, cancellationToken);

        if (orderSearchResult.IsFaulted)
        {
            return new Result<Response>(orderSearchResult.Error);
        }

        OrderEntity order = orderSearchResult.Value;
        Result<OrderEntity> result = existing.Subscribe(order);

        if (result.IsFaulted)
        {
            return new Result<Response>(result.Error);
        }

        _context.OrderSubscriptions.Update(existing);
        await _context.SaveChangesAsync(cancellationToken);

        return default(Response);
    }
}