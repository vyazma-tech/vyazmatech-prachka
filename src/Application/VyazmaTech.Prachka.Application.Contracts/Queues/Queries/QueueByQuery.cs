using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.Dto;
using VyazmaTech.Prachka.Application.Dto.Queue;

namespace VyazmaTech.Prachka.Application.Contracts.Queues.Queries;

public static class QueueByQuery
{
    public record struct Query(DateOnly? SearchFrom, int Page, int Limit) : IQuery<Response>;

    public record struct Response(PagedResponse<QueueDto> Queues);
}