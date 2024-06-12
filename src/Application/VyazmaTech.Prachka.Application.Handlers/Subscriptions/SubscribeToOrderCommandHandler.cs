using VyazmaTech.Prachka.Application.Abstractions.Querying.OrderSubscription;
using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.Core.Errors;
using VyazmaTech.Prachka.Application.Core.Specifications;
using VyazmaTech.Prachka.Application.DataAccess.Contracts;
using VyazmaTech.Prachka.Domain.Common.Exceptions;
using VyazmaTech.Prachka.Domain.Core.Subscriptions;
using static VyazmaTech.Prachka.Application.Contracts.Subscriptions.SubscribeToOrder;

namespace VyazmaTech.Prachka.Application.Handlers.Subscriptions;

internal sealed class SubscribeToOrderCommandHandler : ICommandHandler<Command, Response>
{
    private readonly IPersistenceContext _context;

    public SubscribeToOrderCommandHandler(IPersistenceContext context)
    {
        _context = context;
    }

    public async ValueTask<Response> Handle(Command request, CancellationToken cancellationToken)
    {
        if (request.UserId is null)
            throw new IdentityException(ApplicationErrors.Subscription.AnonymousUserCantSubscribe);

        var query = OrderSubscriptionQuery.Build(x => x.WithUserId(request.UserId.Value));

        OrderSubscriptionEntity? subscription = await _context.OrderSubscriptions
            .QueryAsync(query, cancellationToken)
            .FirstOrDefaultAsync(cancellationToken);

        if (subscription is null)
            throw new NotFoundException(ApplicationErrors.Subscription.UserHasNoSubscriptions(request.UserId.Value));

        Domain.Core.Orders.Order order = await _context.Orders.FindByIdAsync(request.OrderId, cancellationToken);

        subscription.Subscribe(order.Id);

        _context.OrderSubscriptions.Update(subscription);
        await _context.SaveChangesAsync(cancellationToken);

        return default;
    }
}