using VyazmaTech.Prachka.Application.Abstractions.Identity;
using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.Core.Errors;
using VyazmaTech.Prachka.Application.Core.Specifications;
using VyazmaTech.Prachka.Application.DataAccess.Contracts;
using VyazmaTech.Prachka.Domain.Common.Exceptions;
using static VyazmaTech.Prachka.Application.Contracts.Queues.Commands.BulkRemoveOrders;

namespace VyazmaTech.Prachka.Application.Handlers.Queue.Commands.BulkRemoveOrders;

internal sealed class BulkRemoveOrdersCommandHandler : ICommandHandler<Command, Response>
{
    private readonly IPersistenceContext _context;
    private readonly ICurrentUser _currentUser;

    public BulkRemoveOrdersCommandHandler(IPersistenceContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async ValueTask<Response> Handle(Command request, CancellationToken cancellationToken)
    {
        Guid? userId = _currentUser.Id;

        if (userId is null)
            throw new IdentityException(ApplicationErrors.BulkInsertOrders.AnonymousUserCantEnter);

        Domain.Core.Users.User user = await _context.Users.FindByIdAsync(userId.Value, cancellationToken);
        Domain.Core.Queues.Queue queue = await _context.Queues.FindByIdAsync(request.QueueId, cancellationToken);

        queue.RemoveFor(user.Id, request.Quantity);

        await _context.SaveChangesAsync(cancellationToken);

        return default;
    }
}