using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Domain.Common.Result;
using static VyazmaTech.Prachka.Application.Contracts.Subscriptions.SubscribeToQueue;

namespace VyazmaTech.Prachka.Application.Handlers.Subscriptions;

internal sealed class SubscribeToQueueCommandHandler : ICommandHandler<Command, Result<Response>>
{
    public ValueTask<Result<Response>> Handle(Command request, CancellationToken cancellationToken)
    {
        // TODO: implement
        throw new NotImplementedException();
    }
}