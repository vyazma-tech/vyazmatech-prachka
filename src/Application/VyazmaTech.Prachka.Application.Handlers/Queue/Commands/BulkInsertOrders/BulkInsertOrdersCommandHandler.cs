using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Domain.Common.Result;
using static VyazmaTech.Prachka.Application.Contracts.Queues.Commands.BulkInsertOrders;

namespace VyazmaTech.Prachka.Application.Handlers.Queue.Commands.BulkInsertOrders;

internal sealed class BulkInsertOrdersCommandHandler : ICommandHandler<Command, Result<Response>>
{
    public ValueTask<Result<Response>> Handle(Command request, CancellationToken cancellationToken)
    {
        // TODO: implement
        throw new NotImplementedException();
    }
}