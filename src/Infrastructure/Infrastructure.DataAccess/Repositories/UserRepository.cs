using Domain.Common.Errors;
using Domain.Common.Exceptions;
using Domain.Common.Result;
using Domain.Core.User;
using Domain.Core.ValueObjects;
using Infrastructure.DataAccess.Contexts;
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

    public async Task<Result<UserEntity>> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        UserEntity? entity = await DbSet
            .FirstOrDefaultAsync(
                u => u.Id == id,
                cancellationToken);
        
        if (entity is null)
        {
            var exception = new DomainException(DomainErrors.User.NotFound);
            return new Result<UserEntity>(exception);
        }

        return entity;
    }

    public async Task<Result<UserEntity>> FindByTelegramIdAsync(
        TelegramId telegramId,
        CancellationToken cancellationToken)
    {
        UserEntity? entity = await DbSet
            .FirstOrDefaultAsync(
                u => u.TelegramId == telegramId,
                cancellationToken);
        
        if (entity is null)
        {
            var exception = new DomainException(DomainErrors.User.NotFound);
            return new Result<UserEntity>(exception);
        }

        return entity;
    }

    public async Task<Result<IReadOnlyCollection<UserEntity>>> FindByRegistrationDateAsync(
        DateTime registrationDateUtc,
        CancellationToken cancellationToken)
    {
        List<UserEntity> entities = await DbSet
            .Where(u => u.CreationDate == registrationDateUtc)
            .ToListAsync(cancellationToken: cancellationToken);

        return entities;
    }

    public async Task<long> CountAsync(CancellationToken cancellationToken) 
        => await DbSet.CountAsync(cancellationToken);
}