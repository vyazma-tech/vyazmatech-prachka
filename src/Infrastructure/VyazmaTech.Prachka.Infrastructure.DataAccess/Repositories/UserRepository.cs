﻿using Microsoft.EntityFrameworkCore;
using VyazmaTech.Prachka.Application.Abstractions.Querying.User;
using VyazmaTech.Prachka.Application.DataAccess.Contracts.Repositories;
using VyazmaTech.Prachka.Domain.Common.Errors;
using VyazmaTech.Prachka.Domain.Common.Exceptions;
using VyazmaTech.Prachka.Domain.Core.Users;
using VyazmaTech.Prachka.Infrastructure.DataAccess.Contexts;

namespace VyazmaTech.Prachka.Infrastructure.DataAccess.Repositories;

internal sealed class UserRepository : IUserRepository
{
    private readonly DatabaseContext _context;

    public UserRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<User> GetByIdAsync(Guid id, CancellationToken token)
    {
        return await _context.Users.FirstOrDefaultAsync(x => x.Id == id, token)
               ?? throw new NotFoundException(DomainErrors.User.NotFound);
    }

    public IAsyncEnumerable<User> QueryAsync(UserQuery specification, CancellationToken cancellationToken)
    {
        IQueryable<User> queryable = GetSearchQueryable(specification);
        return queryable.ToAsyncEnumerable();
    }

    public void Insert(User user)
        => _context.Users.Add(user);

    public Task<long> CountAsync(UserQuery specification, CancellationToken cancellationToken)
    {
        IQueryable<User> queryable = GetSearchQueryable(specification);
        return queryable.LongCountAsync(cancellationToken);
    }

    private IQueryable<User> GetSearchQueryable(UserQuery specification)
    {
        IQueryable<User> queryable = _context.Users;

        if (specification.Fullname is not null)
            queryable = queryable.Where(x => EF.Functions.ILike(x.Fullname, specification.Fullname));

        if (specification.TelegramUsername is not null)
            queryable = queryable.Where(x => EF.Functions.ILike(x.TelegramUsername, specification.TelegramUsername));

        if (specification.RegistrationDate is not null)
            queryable = queryable.Where(x => x.CreationDate == specification.RegistrationDate);

        queryable = queryable
            .Skip(specification.Page * specification.Limit)
            .Take(specification.Limit);

        return queryable;
    }
}