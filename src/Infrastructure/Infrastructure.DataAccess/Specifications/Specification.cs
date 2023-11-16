using System.Linq.Expressions;
using Domain.Common.Abstractions;

namespace Infrastructure.DataAccess.Specifications;

public abstract class Specification<TEntity> where TEntity : Entity
{
    protected Specification(Expression<Func<TEntity, bool>> criteria)
        => Criteria = criteria;
    
    public Expression<Func<TEntity, bool>> Criteria { get; }
    
    public Expression<Func<TEntity, bool>>? OrderByExpression { get; private set; }

    public Expression<Func<TEntity, bool>>? OrderByDescendingExpression { get; private set; }

    protected void AddOrderBy(Expression<Func<TEntity, bool>> orderByExpression)
        => OrderByExpression = orderByExpression;

    protected void AddOrderByDescending(Expression<Func<TEntity, bool>> orderByDescendingExpression)
        => OrderByDescendingExpression = orderByDescendingExpression;

    public abstract override string ToString();
}