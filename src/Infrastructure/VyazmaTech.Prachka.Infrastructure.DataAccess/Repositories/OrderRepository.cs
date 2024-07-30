using Microsoft.EntityFrameworkCore;
using VyazmaTech.Prachka.Application.DataAccess.Contracts.Repositories;
using VyazmaTech.Prachka.Domain.Common.Errors;
using VyazmaTech.Prachka.Domain.Common.Exceptions;
using VyazmaTech.Prachka.Domain.Core.Orders;
using VyazmaTech.Prachka.Infrastructure.DataAccess.Contexts;

namespace VyazmaTech.Prachka.Infrastructure.DataAccess.Repositories;

internal sealed class OrderRepository : IOrderRepository
{
    private readonly DatabaseContext _context;

    public OrderRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<Order> GetByIdAsync(Guid id, CancellationToken token)
    {
        return await _context.Orders
                   .AsSplitQuery()
                   .Include(x => x.User)
                   .Include(x => x.Queue)
                   .FirstOrDefaultAsync(x => x.Id == id, token)
               ?? throw new NotFoundException(DomainErrors.Order.NotFound);
    }

    public IAsyncEnumerable<Order> QueryByUserAsync(Guid id, CancellationToken token)
    {
        return _context.Orders
            .AsSplitQuery()
            .Include(x => x.User)
            .Include(x => x.Queue)
            .Where(x => x.User.Id == id)
            .AsAsyncEnumerable();
    }

    public void InsertRange(IReadOnlyCollection<Order> orders)
        => _context.Orders.AddRange(orders);

    public void RemoveRange(IReadOnlyCollection<Order> orders)
        => _context.Orders.RemoveRange(orders);
}