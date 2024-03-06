using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using VyazmaTech.Prachka.Infrastructure.DataAccess.Contexts;

namespace VyazmaTech.Prachka.Infrastructure.DataAccess.Repositories;

internal abstract class RepositoryBase<TEntity, TModel>
    where TModel : class
{
    private readonly DatabaseContext _context;

    protected RepositoryBase(DatabaseContext context)
    {
        _context = context;
    }

    protected virtual DbSet<TModel> DbSet => _context.Set<TModel>();

    public void Insert(TEntity entity)
    {
        TModel model = MapFrom(entity);
        DbSet.Add(model);
    }

    public void InsertRange(IReadOnlyCollection<TEntity> entities)
    {
        IEnumerable<TModel> models = entities.Select(MapFrom);
        DbSet.AddRange(models);
    }

    public void Update(TEntity entity)
    {
        EntityEntry<TModel> entry = GetEntry(entity);
        UpdateModel(entry.Entity, entity);

        entry.State = EntityState.Modified;
    }

    public void Remove(TEntity entity)
    {
        EntityEntry<TModel> entry = GetEntry(entity);
        entry.State = entry.State is EntityState.Added ? EntityState.Detached : EntityState.Deleted;
    }

    protected abstract TModel MapFrom(TEntity entity);

    protected abstract bool Equal(TEntity entity, TModel model);

    protected abstract void UpdateModel(TModel model, TEntity entity);

    private EntityEntry<TModel> GetEntry(TEntity entity)
    {
        TModel? existing = DbSet.Local.FirstOrDefault(model => Equal(entity, model));

        return existing is not null
            ? _context.Entry(existing)
            : DbSet.Attach(MapFrom(entity));
    }
}