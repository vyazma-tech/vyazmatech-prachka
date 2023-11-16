using Domain.Common.Abstractions;

namespace Infrastructure.DataAccess.Specifications;

public static class SpecificationEvaluator
{
    public static IQueryable<TEntity> GetQuery<TEntity>(
        IQueryable<TEntity> inputQueryable,
        Specification<TEntity> specification)
        where TEntity : Entity
    {
        IQueryable<TEntity> queryable = inputQueryable;

        queryable = queryable.Where(specification.Criteria);

        if (specification.OrderByExpression is not null) queryable = queryable.OrderBy(specification.OrderByExpression);

        if (specification.OrderByDescendingExpression is not null)
            queryable = queryable.OrderByDescending(specification.OrderByDescendingExpression);

        return queryable;
    }
}