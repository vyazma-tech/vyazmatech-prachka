using Microsoft.EntityFrameworkCore.Storage;

namespace VyazmaTech.Prachka.Application.DataAccess.Contracts;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
}