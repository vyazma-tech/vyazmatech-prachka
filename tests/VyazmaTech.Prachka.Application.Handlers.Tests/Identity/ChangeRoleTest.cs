using FluentAssertions;
using Moq;
using VyazmaTech.Prachka.Application.Abstractions.Identity;
using VyazmaTech.Prachka.Application.Contracts.Identity.Commands;
using VyazmaTech.Prachka.Application.Core.Users;
using VyazmaTech.Prachka.Application.Dto.Identity;
using VyazmaTech.Prachka.Application.Handlers.Identity.Commands.ChangeRole;
using VyazmaTech.Prachka.Application.Handlers.Tests.Fixtures;
using VyazmaTech.Prachka.Application.Handlers.Tests.Identity.ClassData;
using Xunit;

namespace VyazmaTech.Prachka.Application.Handlers.Tests.Identity;

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
            Role: string.Empty,
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