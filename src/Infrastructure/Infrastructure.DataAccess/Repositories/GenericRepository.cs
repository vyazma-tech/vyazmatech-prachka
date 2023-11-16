using Domain.Common.Abstractions;
using Infrastructure.DataAccess.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.DataAccess.Repositories;

internal abstract class GenericRepository<TEntity> where TEntity : Entity
{
    private readonly DatabaseContext _context;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    protected GenericRepository(DatabaseContext context)
        => _context = context;

    /// <summary>
    /// Gets the database set.
    /// </summary>
    protected virtual DbSet<TEntity> DbSet => _context.Set<TEntity>();

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
    
    private EntityEntry<TEntity> GetEntry(TEntity entity)
    {
        TEntity? existing = DbSet.Local.FirstOrDefault(model => entity.Id.Equals(model.Id));

        return existing is not null
            ? _context.Entry(existing)
            : DbSet.Attach(entity);
    }
}