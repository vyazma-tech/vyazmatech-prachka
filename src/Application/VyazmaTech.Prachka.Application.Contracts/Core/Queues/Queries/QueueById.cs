using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.Dto.Core.Queue;

namespace VyazmaTech.Prachka.Application.Contracts.Core.Queues.Queries;

public static class QueueById
{
    public record struct Query(Guid Id) : IQuery<Response>;

    public record struct Response(QueueWithOrdersDto Queue);
}