using Application.DataAccess.Contracts.Repositories;
using Domain.Common.Errors;
using Domain.Common.Exceptions;
using Domain.Common.Result;
using Domain.Core.User;
using Domain.Core.ValueObjects;
using Infrastructure.DataAccess.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccess.Repositories;

internal sealed class UserRepository : GenericRepository, IUserRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserRepository"/> class.
    /// </summary>
    /// <param name="dbContext">The database context.</param>
    public UserRepository(DatabaseContext dbContext)
        : base(dbContext)
    {
    }

    public async Task<Result<UserEntity>> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        UserEntity? entity = await DbContext.Users.
            FirstOrDefaultAsync(u => u.Id == id, cancellationToken: cancellationToken);
        
        if (entity is null)
        {
            var exception = new DomainException(DomainErrors.User.NotFound);
            return new Result<UserEntity>(exception);
        }

        return entity;
    }

    public async Task<Result<UserEntity>> FindByTelegramIdAsync(TelegramId telegramId, CancellationToken cancellationToken)
    {
        UserEntity? entity = await DbContext.Users
            .FirstOrDefaultAsync(u => u.TelegramId == telegramId, cancellationToken: cancellationToken);
        
        if (entity is null)
        {
            var exception = new DomainException(DomainErrors.User.NotFound);
            return new Result<UserEntity>(exception);
        }

        return entity;
    }

    public async Task<Result<IReadOnlyCollection<UserEntity>>> FindByRegistrationDateAsync
        (DateTime registrationDateUtc, CancellationToken cancellationToken)
    {
        List<UserEntity> entities = await DbContext.Users
            .Where(u => u.CreationDate == registrationDateUtc)
            .ToListAsync(cancellationToken: cancellationToken);

        return entities;
    }

    public async Task InsertAsync(UserEntity user, CancellationToken cancellationToken) 
        => await DbContext.Users.AddAsync(user, cancellationToken);

    public async Task<long> CountAsync(CancellationToken cancellationToken) 
        => await DbContext.Users.CountAsync(cancellationToken);
}