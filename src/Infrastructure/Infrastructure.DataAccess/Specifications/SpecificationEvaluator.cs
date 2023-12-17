using Domain.Kernel;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccess.Specifications;

public static class SpecificationEvaluator
{
    public static IQueryable<TEntity> GetQuery<TEntity>(
        IQueryable<TEntity> inputQueryable,
        Specification<TEntity> specification)
        where TEntity : Entity
    {
        IQueryable<TEntity> queryable =
            specification.AsNoTracking ? inputQueryable.AsNoTracking() : inputQueryable;

        queryable = specification.Includes
            .Aggregate(
                queryable,
                (current, include) => specification.AsNoTracking
                    ? current.Include(include).AsNoTracking() : current.Include(include));

        queryable = queryable.Where(specification.Criteria);

        if (specification.OrderByExpression is not null)
        {
            queryable = queryable.OrderBy(specification.OrderByExpression);
        }

        if (specification.OrderByDescendingExpression is not null)
        {
            queryable = queryable.OrderByDescending(specification.OrderByDescendingExpression);
        }

        return queryable;
    }
}