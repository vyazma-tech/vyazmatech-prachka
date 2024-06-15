using Microsoft.EntityFrameworkCore;
using VyazmaTech.Prachka.Application.DataAccess.Contracts.Repositories;

namespace VyazmaTech.Prachka.Application.DataAccess.Contracts;

public interface IPersistenceContext
{
    IQueueRepository Queues { get; }

    IOrderRepository Orders { get; }

    IUserRepository Users { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    DbSet<TModel> Entities<TModel>()
        where TModel : class;
}