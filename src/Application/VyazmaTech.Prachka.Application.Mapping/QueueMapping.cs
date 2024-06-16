using VyazmaTech.Prachka.Application.Dto;
using VyazmaTech.Prachka.Application.Dto.Queue;
using VyazmaTech.Prachka.Domain.Core.Queues;

namespace VyazmaTech.Prachka.Application.Mapping;

public static class QueueMapping
{
    public static QueueDto ToDto(this Queue queue)
    {
        return new QueueDto(
            queue.Id,
            queue.Capacity,
            queue.Orders.Count,
            queue.State.ToString(),
            ModifiedOn: queue.ModifiedOnUtc,
            AssignmentDate: queue.CreationDate,
            ActiveFrom: queue.ActivityBoundaries.ActiveFrom,
            ActiveUntil: queue.ActivityBoundaries.ActiveUntil);
    }

    public static QueueWithOrdersDto ToQueueWithOrdersDto(this Queue queue)
    {
        return new QueueWithOrdersDto(
            queue.Id,
            queue.Capacity,
            queue.Orders.Count,
            queue.Orders.Select(x => x.ToDto()).ToArray(),
            queue.State.ToString(),
            ModifiedOn: queue.ModifiedOnUtc,
            AssignmentDate: queue.AssignmentDate,
            ActiveFrom: queue.ActivityBoundaries.ActiveFrom,
            ActiveUntil: queue.ActivityBoundaries.ActiveUntil);
    }

    public static PagedResponse<QueueDto> ToPagedResponse(
        this IEnumerable<QueueDto> orders,
        int currentPage,
        long totalPages,
        int recordsPerPage)
    {
        return new PagedResponse<QueueDto>
        {
            Bunch = orders.ToArray(),
            CurrentPage = currentPage,
            TotalPages = totalPages,
            RecordPerPage = recordsPerPage,
        };
    }
}