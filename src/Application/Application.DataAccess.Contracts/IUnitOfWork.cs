using Application.DataAccess.Contracts;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.DataAccess.Contracts;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
}