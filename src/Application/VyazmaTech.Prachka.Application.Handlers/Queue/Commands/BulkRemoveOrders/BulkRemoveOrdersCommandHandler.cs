using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Domain.Common.Result;
using static VyazmaTech.Prachka.Application.Contracts.Queues.Commands.BulkRemoveOrders;

namespace VyazmaTech.Prachka.Application.Handlers.Queue.Commands.BulkRemoveOrders;

internal sealed class BulkRemoveOrdersCommandHandler : ICommandHandler<Command, Result<Response>>
{
    public ValueTask<Result<Response>> Handle(Command request, CancellationToken cancellationToken)
    {
        // TODO: implement
        throw new NotImplementedException();
    }
}