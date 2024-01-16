using Domain.Core.Order;
using Domain.Core.Queue;
using Domain.Core.User;

namespace Application.Core.Extensions;

public static class FilteringExtensions
{
    public static IEnumerable<QueueEntity> FilterBy(this IEnumerable<QueueEntity> queues, DateOnly? date)
    {
        return date is null
            ? queues
            : queues.Where(x => x.CreationDate == date);
    }

    public static IEnumerable<UserEntity> FilterBy(
        this IEnumerable<UserEntity> users,
        string? telegramId,
        string? fullname,
        DateOnly? registrationDate)
    {
        IEnumerable<UserEntity> result = users as UserEntity[] ?? users.ToArray();

        result = telegramId is null
            ? result
            : result.Where(x => string.Equals(x.TelegramId.Value, telegramId, StringComparison.InvariantCulture));

        result = fullname is null
            ? result
            : result.Where(x => string.Equals(x.Fullname.Value, fullname, StringComparison.InvariantCultureIgnoreCase));

        result = registrationDate is null
            ? result
            : result.Where(x => x.CreationDate == registrationDate);

        return result;
    }

    public static IEnumerable<OrderEntity> FilterBy(
        this IEnumerable<OrderEntity> orders,
        Guid? userId,
        Guid? queueId,
        DateOnly? creationDate)
    {
        IEnumerable<OrderEntity> result = orders as OrderEntity[] ?? orders.ToArray();

        result = userId is null
            ? result
            : result.Where(x => x.User.Id == userId);

        result = queueId is null
            ? result
            : result.Where(x => x.Queue.Id == queueId);

        result = creationDate is null
            ? result
            : result.Where(x => x.CreationDate == creationDate);

        return result;
    }
}