using VyazmaTech.Prachka.Application.Abstractions.Identity;
using VyazmaTech.Prachka.Application.Contracts.Common;
using VyazmaTech.Prachka.Application.Core.Errors;
using VyazmaTech.Prachka.Application.DataAccess.Contracts;
using VyazmaTech.Prachka.Application.Dto.Core.Order;
using VyazmaTech.Prachka.Domain.Common.Exceptions;
using static VyazmaTech.Prachka.Application.Contracts.Core.Orders.Queries.MyOrders;

namespace VyazmaTech.Prachka.Application.Handlers.Core.Order.Queries;

internal sealed class MyOrdersQueryHandler : IQueryHandler<Query, Response>
{
    private readonly ICurrentUser _currentUser;
    private readonly IPersistenceContext _context;

    public MyOrdersQueryHandler(ICurrentUser currentUser, IPersistenceContext context)
    {
        _currentUser = currentUser;
        _context = context;
    }

    public async ValueTask<Response> Handle(Query request, CancellationToken cancellationToken)
    {
        Guid? userId = _currentUser.Id;

        if (userId is null)
            throw new IdentityException(ApplicationErrors.MyOrders.AnonymousUserCantSeeTheirOrders);

        List<MyOrdersDto> orders = await _context.Orders
            .QueryByUserAsync(userId.Value, cancellationToken)
            .GroupBy(x => x.Queue.AssignmentDate.Value)
            .OrderByDescending(x => x.Key)
            .SelectAwait(async group =>
                new MyOrdersDto(
                    Date: group.Key,
                    Orders: await ToOrdersModel(group, cancellationToken)))
            .ToListAsync(cancellationToken);

        return new Response(orders);
    }

    private static async Task<IReadOnlyCollection<MyOrdersOrderModel>> ToOrdersModel(
        IAsyncGrouping<DateOnly, Domain.Core.Orders.Order> group,
        CancellationToken token)
    {
        return await group.Select(order =>
                new MyOrdersOrderModel(
                    Status: order.Status.ToString(),
                    Comment: order.Comment,
                    CreationDate: order.CreationDate,
                    ModificationDate: order.ModifiedOnUtc?.ToLocalTime(),
                    isNotifyAvailable: false))
            .OrderByDescending(x => x.CreationDate)
            .ToListAsync(token);
    }
}