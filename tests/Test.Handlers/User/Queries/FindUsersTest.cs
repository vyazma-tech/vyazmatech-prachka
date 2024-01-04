using System.Collections.ObjectModel;
using Application.Core.Common;
using Application.Core.Configuration;
using Application.Core.Querying.Abstractions;
using Application.Handlers.Queue.Queries;
using Application.Handlers.User.Queries;
using Domain.Core.User;
using FluentAssertions;
using FluentChaining;
using Infrastructure.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using Test.Core.Domain.Entities.ClassData;
using Test.Handlers.Fixtures;
using Xunit;

namespace Test.Handlers.User.Queries;

public class FindUsersTest : TestBase
{
    private readonly UserQueryHandler _handler;
    
    public FindUsersTest(CoreDatabaseFixture database) : base(database)
    {
        Mock<IEntityFilter<UserEntity, UserQueryParameter>> filter = new();
        Mock<IOptionsMonitor<PaginationConfiguration>> pagination = new();
        IUserRepository userRepository = new UserRepository(database.Context);

        _handler = new UserQueryHandler(filter.Object, pagination.Object, userRepository);
    }

    // [Fact]
    // public async Task FindUsersByNameFilter_WhenOneUserInserted()
    // {
    //     await Database.ResetAsync();
    //     UserEntity user = UserClassData.Create();
    //     Database.Context.Add(user);
    //     await Database.Context.SaveChangesAsync();
    //     
    //     UserQueryParameter queryParameters = new[]
    //     {
    //         (
    //             UserQueryParameter.Fullname,
    //             user.Fullname.Value
    //         )
    //     };
    //     var readOnlyCollection = new ReadOnlyCollection<QueryParameter<UserQueryParameter>>(queryParameters);
    //     var conf = new QueryConfiguration<UserQueryParameter>(queryParameters);
    //
    //     var userQuery = new UserQuery(null, user.Fullname.Value, null, null, 1)
    //     {
    //         Configuration = conf
    //     };
    //     PagedResponse<UserResponse> response = await _handler.Handle(userQuery, CancellationToken.None);
    //     response.Should().NotBeNull();
    //     response.Bunch.Count.Should().Be(1);
    // }
}