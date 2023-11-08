using Infrastructure.DataAccess.Contexts;

namespace Infrastructure.DataAccess.Repositories;

internal abstract class GenericRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GenericRepository"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    protected GenericRepository(DatabaseContext dbContext) => DbContext = dbContext;

    /// <summary>
    /// Gets the database context.
    /// </summary>
    protected DatabaseContext DbContext { get; }
}