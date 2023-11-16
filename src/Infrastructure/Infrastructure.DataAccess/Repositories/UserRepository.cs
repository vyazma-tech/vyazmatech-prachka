using Domain.Common.Abstractions;
using Domain.Common.Errors;
using Domain.Common.Exceptions;
using Domain.Common.Result;
using Domain.Core.User;
using Domain.Core.ValueObjects;
using Infrastructure.DataAccess.Contexts;
using Infrastructure.DataAccess.Specifications.User;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccess.Repositories;

internal sealed class UserRepository : GenericRepository<UserEntity>, IUserRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserRepository"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public UserRepository(DatabaseContext context)
        : base(context)
    {
    }

    public async Task<long> CountAsync(CancellationToken cancellationToken)
        => await DbSet.CountAsync(cancellationToken);
}