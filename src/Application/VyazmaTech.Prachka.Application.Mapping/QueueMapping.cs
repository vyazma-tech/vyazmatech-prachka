using VyazmaTech.Prachka.Application.Dto;
using VyazmaTech.Prachka.Application.Dto.Queue;
using VyazmaTech.Prachka.Domain.Core.Queue;

namespace VyazmaTech.Prachka.Application.Mapping;

public static class QueueMapping
{
    public static QueueDto ToDto(this QueueEntity queue)
    {
        return new QueueDto(
            Id: queue.Id,
            MaxCapacity: queue.Capacity,
            CurrentCapacity: queue.Order.Count,
            State: queue.State.ToString(),
            ModifiedOn: queue.ModifiedOn?.Value,
            AssignmentDate: queue.CreationDate,
            ActiveFrom: queue.ActiveFrom,
            ActiveUntil: queue.ActiveUntil);
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
            RecordPerPage = recordsPerPage
        };
    }
}