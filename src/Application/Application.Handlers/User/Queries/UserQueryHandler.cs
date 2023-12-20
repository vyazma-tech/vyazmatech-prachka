using System.Collections.ObjectModel;
using Application.Core.Common;
using Application.Core.Configuration;
using Application.Core.Contracts;
using Application.Handlers.Mapping.UserMapping;
using Domain.Common.Result;
using Domain.Core.User;
using Domain.Core.ValueObjects;
using Infrastructure.DataAccess.Contracts;
using Infrastructure.DataAccess.Quering.Abstractions;
using Infrastructure.DataAccess.Specifications.User;
using Microsoft.Extensions.Options;

namespace Application.Handlers.User.Queries;

internal sealed class UserQueryHandler : IQueryHandler<UserQuery, PagedResponse<UserResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly PaginationConfiguration _paginationConfiguration;
    
    private readonly IModelFilter<UserEntity, UserQueryParameter> _filter;

    public UserQueryHandler(
        IModelFilter<UserEntity, UserQueryParameter> filter,
        IOptionsMonitor<PaginationConfiguration> paginationConfiguration,
        IUserRepository userRepository)
    {
        _filter = filter;
        _userRepository = userRepository;
        _paginationConfiguration = paginationConfiguration.CurrentValue;
    }

    public async ValueTask<PagedResponse<UserResponse>> Handle(UserQuery request, CancellationToken cancellationToken)
    {
        long totalRecords = await _userRepository.CountAsync(cancellationToken);
        var users = new List<UserEntity>();
        if (request.Id.HasValue)
        {
            var idSpec = new UserByIdSpecification(request.Id.Value);
            IReadOnlyCollection<UserEntity> userResult = await _userRepository.FindAllByAsync(idSpec, cancellationToken);
            users.AddRange(userResult);
        }
        
        if (request.RegistrationDate.HasValue)
        {
            DateOnly creationDateUtc = request.RegistrationDate.Value;
            var creationDateSpec = new UserByRegistrationDateSpecification(creationDateUtc);
            IReadOnlyCollection<UserEntity> userResult = await _userRepository
                .FindAllByAsync(creationDateSpec, cancellationToken);
            users.AddRange(users.Any() ? userResult.Where(o => users.Contains(o)) : userResult);
        }

        users = _filter.Apply(users, request.Configuration).ToList();

        var readonlyUsers = new ReadOnlyCollection<UserResponse>(users
            .Select(user => new UserResponse(user.ToDto())).ToList());
       
        return new PagedResponse<UserResponse>
        {
            Bunch = readonlyUsers,
            TotalPages = totalRecords / _paginationConfiguration.RecordsPerPage,
            RecordPerPage = _paginationConfiguration.RecordsPerPage,
            CurrentPage = request.Page ?? 1
        };
    }
}