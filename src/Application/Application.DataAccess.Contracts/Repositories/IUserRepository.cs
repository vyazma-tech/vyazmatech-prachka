using Domain.Common.Result;
using Domain.Core.User;
using Domain.Core.ValueObjects;

namespace Application.DataAccess.Contracts.Repositories;

public interface IUserRepository
{
    Task<Result<UserEntity>> FindByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<Result<UserEntity>> FindByTelegramIdAsync(TelegramId telegramId, CancellationToken cancellationToken);

    Task<Result<IReadOnlyCollection<UserEntity>>> FindByRegistrationDateAsync(DateTime registrationDateUtc, CancellationToken cancellationToken);

    Task InsertAsync(UserEntity user, CancellationToken cancellationToken);

    Task<long> CountAsync(CancellationToken cancellationToken);
}