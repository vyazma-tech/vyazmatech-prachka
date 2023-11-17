using Domain.Common.Abstractions;
using Domain.Common.Result;

namespace Domain.Core.User;

public interface IUserRepository
{
    Task<Result<UserEntity>> FindByAsync(
        Specification<UserEntity> specification,
        CancellationToken cancellationToken);

    Task<IReadOnlyCollection<UserEntity>> FindAllByAsync(
        Specification<UserEntity> specification,
        CancellationToken cancellationToken);

    void Insert(UserEntity user);

    Task<long> CountAsync(CancellationToken cancellationToken);
}