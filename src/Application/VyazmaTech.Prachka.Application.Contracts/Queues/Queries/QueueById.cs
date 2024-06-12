using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.Dto.Queue;

namespace VyazmaTech.Prachka.Application.Contracts.Queues.Queries;

public static class QueueById
{
    public record struct Query(Guid Id) : IQuery<Response>;

    public record struct Response(QueueWithOrdersDto Queue);
}