using Domain.Common.Errors;
using Domain.Common.Exceptions;
using Domain.Common.Result;
using Domain.Kernel;
using Infrastructure.DataAccess.Contexts;
using Infrastructure.DataAccess.Contracts;
using Infrastructure.DataAccess.Specifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.DataAccess.Repositories;

internal class GenericRepository<TEntity> : IRepository<TEntity>
    where TEntity : Entity
{
    private readonly DatabaseContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="GenericRepository{TEntity}" /> class.
    /// </summary>
    /// <param name="context">database context.</param>
    internal GenericRepository(DatabaseContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Gets the database set.
    /// </summary>
    protected virtual DbSet<TEntity> DbSet => _context.Set<TEntity>();

    public async Task<Result<TEntity>> FindByAsync(
        Specification<TEntity> specification,
        CancellationToken cancellationToken)
    {
        TEntity? entity = await
            ApplySpecification(specification)
                .FirstOrDefaultAsync(cancellationToken);

        if (entity is null)
        {
            var exception = new DomainException(DomainErrors.Entity.NotFoundFor<TEntity>(specification.ToString()));
            return new Result<TEntity>(exception);
        }

        return entity;
    }

    public async Task<IReadOnlyCollection<TEntity>> FindAllByAsync(
        Specification<TEntity> specification,
        CancellationToken cancellationToken)
    {
        return await ApplySpecification(specification)
            .ToListAsync(cancellationToken);
    }

    public void Insert(TEntity entity)
        => DbSet.Add(entity);

    public async Task InsertRangeAsync(IReadOnlyCollection<TEntity> entities, CancellationToken cancellationToken)
        => await DbSet.AddRangeAsync(entities, cancellationToken);

    public void Update(TEntity entity)
        => DbSet.Update(entity);

    public void Remove(TEntity entity)
    {
        EntityEntry<TEntity> entry = GetEntry(entity);
        entry.State = entry.State is EntityState.Added ? EntityState.Detached : EntityState.Deleted;
    }

    protected IQueryable<TEntity> ApplySpecification(Specification<TEntity> specification)
        => SpecificationEvaluator.GetQuery(DbSet, specification);

    private EntityEntry<TEntity> GetEntry(TEntity entity)
    {
        TEntity? existing = DbSet.Local.FirstOrDefault(model => entity.Id.Equals(model.Id));

        return existing is not null
            ? _context.Entry(existing)
            : DbSet.Attach(entity);
    }
}