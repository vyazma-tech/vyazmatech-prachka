using VyazmaTech.Prachka.Application.Abstractions.Querying.QueueSubscription;
using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.Core.Errors;
using VyazmaTech.Prachka.Application.Core.Specifications;
using VyazmaTech.Prachka.Application.DataAccess.Contracts;
using VyazmaTech.Prachka.Domain.Common.Exceptions;
using VyazmaTech.Prachka.Domain.Core.Subscriptions;
using static VyazmaTech.Prachka.Application.Contracts.Subscriptions.SubscribeToQueue;

namespace VyazmaTech.Prachka.Application.Handlers.Subscriptions;

internal sealed class SubscribeToQueueCommandHandler : ICommandHandler<Command, Response>
{
    private readonly IPersistenceContext _context;

    public SubscribeToQueueCommandHandler(IPersistenceContext context)
    {
        _context = context;
    }

    public async ValueTask<Response> Handle(Command request, CancellationToken cancellationToken)
    {
        if (request.UserId is null)
            throw new IdentityException(ApplicationErrors.Subscription.AnonymousUserCantSubscribe);

        var query = QueueSubscriptionQuery.Build(x => x.WithUserId(request.UserId.Value));

        QueueSubscriptionEntity? subscription = await _context.QueueSubscriptions
            .QueryAsync(query, cancellationToken)
            .FirstOrDefaultAsync(cancellationToken);

        if (subscription is null)
            throw new NotFoundException(ApplicationErrors.Subscription.UserHasNoSubscriptions(request.UserId.Value));

        Domain.Core.Queues.Queue queue = await _context.Queues.FindByIdAsync(request.QueueId, cancellationToken);

        subscription.Subscribe(queue.Id);

        _context.QueueSubscriptions.Update(subscription);
        await _context.SaveChangesAsync(cancellationToken);

        return default;
    }
}