using System.Linq.Expressions;

namespace Domain.Kernel;

public abstract class Specification<TEntity>
    where TEntity : Entity
{
    protected Specification(Expression<Func<TEntity, bool>> criteria)
    {
        Criteria = criteria;
    }

    public Expression<Func<TEntity, bool>> Criteria { get; }
    
    public bool AsNoTracking { get; set; }
    
    public List<Expression<Func<TEntity, object>>> Includes { get; } = new ();

    public Expression<Func<TEntity, bool>>? OrderByExpression { get; private set; }

    public Expression<Func<TEntity, bool>>? OrderByDescendingExpression { get; private set; }

    public abstract override string ToString();

    public void AddInclude(Expression<Func<TEntity, object>> includeExpression)
    {
        Includes.Add(includeExpression);
    }

    protected void AddOrderBy(Expression<Func<TEntity, bool>> orderByExpression)
    {
        OrderByExpression = orderByExpression;
    }

    protected void AddOrderByDescending(Expression<Func<TEntity, bool>> orderByDescendingExpression)
    {
        OrderByDescendingExpression = orderByDescendingExpression;
    }
}