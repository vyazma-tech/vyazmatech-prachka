using Application.DataAccess.Contracts;
using Domain.Core.Queue;
using Domain.Kernel;

namespace Application.Core.Services;

public class QueueUpdatingService : IDisposable
{
    private readonly SemaphoreSlim _semaphore = new (1, 1);

    public async Task<UpdateResult> UpdateQueueAsync(
        QueueEntity queue,
        IDateTimeProvider timeProvider,
        CancellationToken token,
        IPersistenceContext context)
    {
        await _semaphore.WaitAsync(token);
        UpdateResult updateResult = queue.State switch
        {
            QueueState.Active => await Expire(queue, timeProvider),
            QueueState.Prepared => await Activate(queue, timeProvider),
            QueueState.Expired => await NotifySubscribers(queue, timeProvider),

            _ => throw new InvalidOperationException("Unknown queue state.")
        };

        if (updateResult.Modified)
        {
            context.Queues.Update(queue);
            await context.SaveChangesAsync(token);
        }

        _semaphore.Release();

        return updateResult;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _semaphore.Dispose();
    }

    private Task<UpdateResult> Activate(QueueEntity queue, IDateTimeProvider timeProvider)
    {
        if (queue.TryActivate(timeProvider.SpbDateTimeNow))
            return Task.FromResult(new UpdateResult(QueueState.Prepared, true));

        return Task.FromResult(new UpdateResult(queue.State, false));
    }

    private Task<UpdateResult> Expire(QueueEntity queue, IDateTimeProvider timeProvider)
    {
        if (queue.TryExpire(timeProvider.SpbDateTimeNow))
            return Task.FromResult(new UpdateResult(QueueState.Active, true));

        return Task.FromResult(new UpdateResult(queue.State, false));
    }

    private Task<UpdateResult> NotifySubscribers(QueueEntity queue, IDateTimeProvider timeProvider)
    {
        if (queue.TryNotifyAboutAvailablePosition(timeProvider.SpbDateTimeNow))
            return Task.FromResult(new UpdateResult(QueueState.Expired, true));

        return Task.FromResult(new UpdateResult(queue.State, false));
    }
}

public readonly record struct UpdateResult(QueueState PreviousState, bool Modified);