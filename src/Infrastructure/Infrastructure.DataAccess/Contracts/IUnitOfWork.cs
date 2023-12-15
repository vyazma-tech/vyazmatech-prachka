using Domain.Kernel;
using Infrastructure.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.DataAccess.Contracts;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);

    IRepository<TEntity> GetRepository<TEntity>()
        where TEntity : Entity;
}