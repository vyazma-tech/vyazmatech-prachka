using VyazmaTech.Prachka.Application.Abstractions.Querying.QueueSubscription;
using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.Core.Errors;
using VyazmaTech.Prachka.Application.Core.Specifications;
using VyazmaTech.Prachka.Application.DataAccess.Contracts;
using VyazmaTech.Prachka.Domain.Common.Result;
using VyazmaTech.Prachka.Domain.Core.Queue;
using VyazmaTech.Prachka.Domain.Core.Subscription;
using static VyazmaTech.Prachka.Application.Contracts.Subscriptions.SubscribeToQueue;

namespace VyazmaTech.Prachka.Application.Handlers.Subscriptions;

internal sealed class SubscribeToQueueCommandHandler : ICommandHandler<Command, Result<Response>>
{
    private readonly IPersistenceContext _context;

    public SubscribeToQueueCommandHandler(IPersistenceContext context)
    {
        _context = context;
    }

    public async ValueTask<Result<Response>> Handle(Command request, CancellationToken cancellationToken)
    {
        if (request.UserId is null)
        {
            return new Result<Response>(ApplicationErrors.Subscription.AnonymousUserCantSubscribe);
        }

        var query = QueueSubscriptionQuery.Build(x => x.WithUserId(request.UserId.Value));
        QueueSubscriptionEntity? existing = await _context.QueueSubscriptions
            .QueryAsync(query, cancellationToken)
            .FirstOrDefaultAsync(cancellationToken);

        if (existing is null)
        {
            return new Result<Response>(ApplicationErrors.Subscription.UserHasNoSubscriptions(request.UserId.Value));
        }

        Result<QueueEntity> queueSearchResult = await _context.Queues.FindByIdAsync(request.QueueId, cancellationToken);

        if (queueSearchResult.IsFaulted)
        {
            return new Result<Response>(queueSearchResult.Error);
        }

        QueueEntity order = queueSearchResult.Value;
        Result<QueueEntity> result = existing.Subscribe(order);

        if (result.IsFaulted)
        {
            return new Result<Response>(result.Error);
        }

        _context.QueueSubscriptions.Update(existing);
        await _context.SaveChangesAsync(cancellationToken);

        return default(Response);
    }
}