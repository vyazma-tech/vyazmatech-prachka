using Application.DataAccess.Contracts;
using Infrastructure.DataAccess.Contracts;

namespace Infrastructure.DataAccess.Specifications;

public static class SpecificationEvaluator
{
    public static IQueryable<TModel> GetQuery<TModel>(
        IQueryable<TModel> inputQueryable,
        Specification<TModel> specification)
        where TModel : class
    {
        IQueryable<TModel> queryable = inputQueryable;

        if (specification.Criteria is not null)
        {
            queryable = queryable.Where(specification.Criteria);
        }

        if (specification is { Page: { } page, RecordsPerPage: { } recordsPerPage })
        {
            queryable = queryable
                .Skip(page * recordsPerPage)
                .Take(recordsPerPage);
        }

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