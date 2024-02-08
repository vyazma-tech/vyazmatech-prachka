using Application.Core.Authentication;
using Application.Core.Contracts.Identity.Commands;
using Application.DataAccess.Contracts;
using Application.Dto.Identity;
using Application.Handlers.Identity.Commands.ChangeRole;
using FluentAssertions;
using Moq;
using Test.Handlers.Fixtures;
using Test.Handlers.Identity.ClassData;
using Xunit;

namespace Test.Handlers.Identity;

public class ChangeRoleTest : TestBase
{
    private readonly Mock<IAuthenticationService> _authService;

    public ChangeRoleTest(CoreDatabaseFixture fixture) : base(fixture)
    {
        _authService = new Mock<IAuthenticationService>();
    }

    [Theory]
    [ClassData(typeof(ChangeRoleClassData))]
    public async Task Handle_ShouldReturnSuccessResult_WhenAdminChangeRole(string currentRole, string newRole)
    {
        var user = new IdentityUserDto(
            Id: Guid.Empty,
            Fullname: string.Empty,
            TelegramUsername: string.Empty,
            TelegramImageUrl: string.Empty);

        var currentUser = new AdminUser(user.Id);

        _authService
            .Setup(x => x.GetUserRoleAsync(user.TelegramUsername, default))
            .ReturnsAsync(currentRole);

        var command = new ChangeRole.Command(user.TelegramUsername, newRole);
        var handler = new ChangeRoleCommandHandler(_authService.Object, currentUser);

        ChangeRole.Response handlerResult = await handler.Handle(command, default);

        handlerResult.Result.IsFaulted.Should().BeFalse();
    }
}