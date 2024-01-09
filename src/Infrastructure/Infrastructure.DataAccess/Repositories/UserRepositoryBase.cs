using Domain.Core.User;
using Infrastructure.DataAccess.Contexts;
using Infrastructure.DataAccess.Contracts;
using Infrastructure.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccess.Repositories;

internal sealed class UserRepositoryBase : RepositoryBase<UserEntity, UserModel>, IUserRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserRepositoryBase" /> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public UserRepositoryBase(DatabaseContext context)
        : base(context)
    {
    }

    public Task<long> CountAsync(Specification<UserModel> specification, CancellationToken cancellationToken)
    {
        IQueryable<UserModel> queryable = ApplySpecification(specification);
        return queryable.LongCountAsync(cancellationToken);
    }

    protected override UserModel MapFrom(UserEntity entity)
    {
        throw new NotImplementedException();
    }

    protected override UserEntity MapTo(UserModel model)
    {
        throw new NotImplementedException();
    }

    protected override bool Equal(UserEntity entity, UserModel model)
    {
        return entity.Id.Equals(model.Id);
    }

    protected override void UpdateModel(UserModel model, UserEntity entity)
    {
        throw new NotImplementedException();
    }
}