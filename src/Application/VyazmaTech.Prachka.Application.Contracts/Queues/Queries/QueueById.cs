using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.Dto.Queue;
using VyazmaTech.Prachka.Domain.Common.Result;

namespace VyazmaTech.Prachka.Application.Contracts.Queues.Queries;

public static class QueueById
{
    public record struct Query(Guid Id) : IQuery<Result<Response>>;

    public record struct Response(QueueWithOrdersDto Queue);
}