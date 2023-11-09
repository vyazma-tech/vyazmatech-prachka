using Domain.Common.Result;
using Domain.Core.ValueObjects;

namespace Domain.Core.User;

public interface IUserRepository
{
    Task<Result<UserEntity>> FindByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<Result<UserEntity>> FindByTelegramIdAsync(TelegramId telegramId, CancellationToken cancellationToken);

    Task<Result<IReadOnlyCollection<UserEntity>>> FindByRegistrationDateAsync(DateTime registrationDateUtc, CancellationToken cancellationToken);

    Task InsertAsync(UserEntity user, CancellationToken cancellationToken);

    Task<long> CountAsync(CancellationToken cancellationToken);
}