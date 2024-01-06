using Domain.Core.Queue;

namespace Application.Core.Extensions;

public static class FilteringExtensions
{
    public static IEnumerable<QueueEntity> FilterBy(this IEnumerable<QueueEntity> queues, DateOnly? date)
    {
        return date is null
            ? queues
            : queues.Where(x => x.CreationDate == date);
    }
}