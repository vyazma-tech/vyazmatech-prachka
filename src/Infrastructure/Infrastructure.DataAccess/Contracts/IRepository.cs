using Domain.Common.Result;
using Domain.Kernel;

namespace Infrastructure.DataAccess.Contracts;

public interface IRepository<TEntity>
    where TEntity : Entity
{
    Task<Result<TEntity>> FindByAsync(
        Specification<TEntity> specification,
        CancellationToken cancellationToken);

    Task<IReadOnlyCollection<TEntity>> FindAllByAsync(
        Specification<TEntity> specification,
        CancellationToken cancellationToken);

    void Insert(TEntity entity);

    Task InsertRangeAsync(IReadOnlyCollection<TEntity> entities, CancellationToken cancellationToken);

    void Update(TEntity entity);

    void Remove(TEntity entity);
}