using Microsoft.EntityFrameworkCore;
using VyazmaTech.Prachka.Application.Abstractions.Querying.Queue;
using VyazmaTech.Prachka.Application.DataAccess.Contracts.Repositories;
using VyazmaTech.Prachka.Domain.Common.Errors;
using VyazmaTech.Prachka.Domain.Common.Exceptions;
using VyazmaTech.Prachka.Domain.Core.Queues;
using VyazmaTech.Prachka.Domain.Core.ValueObjects;
using VyazmaTech.Prachka.Infrastructure.DataAccess.Contexts;

namespace VyazmaTech.Prachka.Infrastructure.DataAccess.Repositories;

internal sealed class QueueRepository : IQueueRepository
{
    private readonly DatabaseContext _context;

    public QueueRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<Queue> GetByIdAsync(Guid id, CancellationToken token)
    {
        return await _context.Queues
                   .AsSplitQuery()
                   .Include(x => x.Orders)
                   .ThenInclude(x => x.User)
                   .FirstOrDefaultAsync(x => x.Id == id, token)
               ?? throw new NotFoundException(DomainErrors.Queue.NotFound);
    }

    public async Task<Queue?> FindByAssignmentDate(AssignmentDate assignmentDate, CancellationToken token)
        => await _context.Queues.FirstOrDefaultAsync(x => x.AssignmentDate == assignmentDate, token);

    public IAsyncEnumerable<Queue> QueryByTelegramUsername(
        TelegramUsername username,
        DateOnly searchFrom)
    {
        IQueryable<Queue> queues =
            from user in _context.Users.AsNoTracking()
            join order in _context.Orders.AsNoTracking()
                on user.Id equals order.User.Id
            join queue in _context.Queues.AsNoTracking()
                on order.Queue.Id equals queue.Id
            where user.TelegramUsername == username && queue.AssignmentDate >= searchFrom
            orderby queue.AssignmentDate
            select queue;

        return queues.AsSplitQuery().AsAsyncEnumerable();
    }

    public IAsyncEnumerable<Queue> QueryFromAsync(QueueQuery specification)
    {
        IQueryable<Queue> queryable = GetSearchQueryable(specification);
        return queryable.AsAsyncEnumerable();
    }

    public void InsertRange(IReadOnlyCollection<Queue> queues)
        => _context.AddRange(queues);

    public Task<long> CountAsync(QueueQuery specification, CancellationToken cancellationToken)
    {
        IQueryable<Queue> queryable = GetSearchQueryable(specification);
        return queryable.LongCountAsync(cancellationToken);
    }

    private IQueryable<Queue> GetSearchQueryable(QueueQuery specification)
    {
        IQueryable<Queue> queryable = _context.Queues;

        queryable = queryable.Where(x => x.AssignmentDate >= specification.SearchFrom)
            .Skip(specification.Page * specification.Limit)
            .Take(specification.Limit)
            .OrderByDescending(x => x.AssignmentDate);

        return queryable;
    }
}