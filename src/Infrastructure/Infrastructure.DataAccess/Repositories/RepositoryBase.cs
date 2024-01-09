using Domain.Common.Errors;
using Domain.Common.Exceptions;
using Domain.Common.Result;
using Infrastructure.DataAccess.Contexts;
using Infrastructure.DataAccess.Contracts;
using Infrastructure.DataAccess.Specifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.DataAccess.Repositories;

internal abstract class RepositoryBase<TEntity, TModel>
    where TModel : class
{
    private readonly DatabaseContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="RepositoryBase{TEntity,TModel}"/> class.
    /// </summary>
    /// <param name="context">database context.</param>
    protected RepositoryBase(DatabaseContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Gets the database set.
    /// </summary>
    protected virtual DbSet<TModel> DbSet => _context.Set<TModel>();

    public async Task<Result<TEntity>> FindByAsync(
        Specification<TModel> specification,
        CancellationToken cancellationToken)
    {
        TModel? model = await
            ApplySpecification(specification)
                .FirstOrDefaultAsync(cancellationToken);

        if (model is null)
        {
            var exception = new DomainException(DomainErrors.Entity.NotFoundFor<TEntity>(specification.ToString()));
            return new Result<TEntity>(exception);
        }

        return MapTo(model);
    }

    public IAsyncEnumerable<TEntity> FindAllByAsync(
        Specification<TModel> specification,
        CancellationToken cancellationToken)
    {
        return ApplySpecification(specification)
            .AsAsyncEnumerable()
            .Select(MapTo);
    }

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

    protected abstract TEntity MapTo(TModel model);

    protected abstract bool Equal(TEntity entity, TModel model);

    protected abstract void UpdateModel(TModel model, TEntity entity);

    protected IQueryable<TModel> ApplySpecification(Specification<TModel> specification)
        => SpecificationEvaluator.GetQuery(DbSet, specification);

    private EntityEntry<TModel> GetEntry(TEntity entity)
    {
        TModel? existing = DbSet.Local.FirstOrDefault(model => Equal(entity, model));

        return existing is not null
            ? _context.Entry(existing)
            : DbSet.Attach(MapFrom(entity));
    }
}