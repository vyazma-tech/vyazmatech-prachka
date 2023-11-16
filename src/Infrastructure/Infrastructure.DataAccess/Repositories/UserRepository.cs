using Domain.Core.User;
using Infrastructure.DataAccess.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccess.Repositories;

internal sealed class UserRepository : GenericRepository<UserEntity>, IUserRepository
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="UserRepository" /> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public UserRepository(DatabaseContext context)
        : base(context)
    {
    }

    public async Task<long> CountAsync(CancellationToken cancellationToken)
    {
        return await DbSet.CountAsync(cancellationToken);
    }
}