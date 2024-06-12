using Microsoft.EntityFrameworkCore;
using VyazmaTech.Prachka.Application.Abstractions.Querying.User;
using VyazmaTech.Prachka.Application.DataAccess.Contracts.Repositories;
using VyazmaTech.Prachka.Domain.Core.User;
using VyazmaTech.Prachka.Infrastructure.DataAccess.Contexts;
using VyazmaTech.Prachka.Infrastructure.DataAccess.Mapping;
using VyazmaTech.Prachka.Infrastructure.DataAccess.Models;

namespace VyazmaTech.Prachka.Infrastructure.DataAccess.Repositories;

internal sealed class UserRepository : RepositoryBase<UserEntity, UserModel>, IUserRepository
{
    public UserRepository(DatabaseContext context)
        : base(context) { }

    public IAsyncEnumerable<UserEntity> QueryAsync(UserQuery specification, CancellationToken cancellationToken)
    {
        IQueryable<UserModel> queryable = ApplyQuery(specification);

        return queryable.AsAsyncEnumerable().Select(MapTo);
    }

    public Task<long> CountAsync(UserQuery specification, CancellationToken cancellationToken)
    {
        IQueryable<UserModel> queryable = ApplyQuery(specification);
        return queryable.LongCountAsync(cancellationToken);
    }

    protected override UserModel MapFrom(UserEntity entity)
    {
        return UserMapping.MapFrom(entity);
    }

    protected override bool Equal(UserEntity entity, UserModel model)
    {
        return entity.Id.Equals(model.Id);
    }

    protected override void UpdateModel(UserModel model, UserEntity entity)
    {
        model.Fullname = entity.Fullname;
        model.ModifiedOn = entity.ModifiedOnUtc;
    }

    private static UserEntity MapTo(UserModel model)
    {
        return UserMapping.MapTo(model);
    }

    private IQueryable<UserModel> ApplyQuery(UserQuery specification)
    {
        IQueryable<UserModel> queryable = DbSet;

        if (specification.Id is not null)
        {
            queryable = queryable.Where(x => x.Id == specification.Id);
        }

        if (specification.TelegramId is not null)
        {
            queryable = queryable.Where(x => EF.Functions.ILike(x.TelegramUsername, specification.TelegramId));
        }

        if (specification.Fullname is not null)
        {
            queryable = queryable.Where(x => EF.Functions.ILike(x.Fullname, specification.Fullname));
        }

        if (specification.RegistrationDate is not null)
        {
            queryable = queryable.Where(x => x.RegistrationDate == specification.RegistrationDate);
        }

        if (specification.Limit is not null)
        {
            if (specification.Page is not null)
            {
                queryable = queryable.Skip(specification.Page.Value * specification.Limit.Value)
                    .Take(specification.Limit.Value);
            }
            else
            {
                queryable = queryable.Take(specification.Limit.Value);
            }
        }

        return queryable;
    }
}