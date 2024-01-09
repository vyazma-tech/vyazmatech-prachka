using Domain.Core.User;
using Infrastructure.DataAccess.Contexts;
using Infrastructure.DataAccess.Contracts;
using Infrastructure.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccess.Repositories;

internal sealed class UserRepository : RepositoryBase<UserEntity, UserModel>, IUserRepository
{
    /// <inheritdoc cref="RepositoryBase{TEntity,TModel}"/>
    public UserRepository(DatabaseContext context)
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