using Domain.Common.Result;
using Domain.Core.User;
using Infrastructure.DataAccess.Models;

namespace Infrastructure.DataAccess.Contracts;

public interface IUserRepository
{
    Task<Result<UserEntity>> FindByAsync(
        Specification<UserModel> specification,
        CancellationToken cancellationToken);

    IAsyncEnumerable<UserEntity> FindAllByAsync(
        Specification<UserModel> specification,
        CancellationToken cancellationToken);

    void Insert(UserEntity user);

    Task<long> CountAsync(Specification<UserModel> specification, CancellationToken cancellationToken);
}