using System.Linq.Expressions;

namespace Infrastructure.DataAccess.Contracts;

public abstract class Specification<TModel>
    where TModel : class
{
    protected Specification(Expression<Func<TModel, bool>> criteria)
    {
        Criteria = criteria;
    }

    public Expression<Func<TModel, bool>> Criteria { get; }

    public Expression<Func<TModel, bool>>? OrderByExpression { get; private set; }

    public Expression<Func<TModel, bool>>? OrderByDescendingExpression { get; private set; }

    public abstract override string ToString();

    protected void AddOrderBy(Expression<Func<TModel, bool>> orderByExpression)
    {
        OrderByExpression = orderByExpression;
    }

    protected void AddOrderByDescending(Expression<Func<TModel, bool>> orderByDescendingExpression)
    {
        OrderByDescendingExpression = orderByDescendingExpression;
    }
}