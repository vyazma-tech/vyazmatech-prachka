using FluentAssertions;
using Moq;
using VyazmaTech.Prachka.Application.Abstractions.Identity;
using VyazmaTech.Prachka.Application.Contracts.Identity.Commands;
using VyazmaTech.Prachka.Application.Core.Users;
using VyazmaTech.Prachka.Application.Dto.Identity;
using VyazmaTech.Prachka.Application.Handlers.Identity.Commands.ChangeRole;
using VyazmaTech.Prachka.Application.Handlers.Tests.Fixtures;
using VyazmaTech.Prachka.Application.Handlers.Tests.Identity.ClassData;
using VyazmaTech.Prachka.Domain.Common.Exceptions;

namespace VyazmaTech.Prachka.Application.Handlers.Tests.Identity;

public class ChangeRoleTest : TestBase
{
    private readonly Mock<IAuthenticationService> _authService;

    public ChangeRoleTest(CoreDatabaseFixture fixture) : base(fixture)
    {
        _authService = new Mock<IAuthenticationService>();
    }

    [Theory]
    [ClassData(typeof(AdminChangeRoleClassData))]
    [ClassData(typeof(ModeratorChangeRoleClassData))]
    public async Task Handle_ShouldNotThrow_WhenAdminChangeRole(string currentRole, string newRole)
    {
        var user = new IdentityUserDto(
            Guid.Empty,
            string.Empty,
            string.Empty,
            string.Empty);

        var currentUser = new AdminUser(user.Id);

        _authService
            .Setup(x => x.GetUserRoleAsync(user.TelegramUsername, default))
            .ReturnsAsync(currentRole);

        var command = new ChangeRole.Command(user.TelegramUsername, newRole);
        var handler = new ChangeRoleCommandHandler(_authService.Object, currentUser);

        Func<Task<ChangeRole.Response>> action = async () => await handler.Handle(command, default);

        await action.Should().NotThrowAsync();
    }

    [Theory]
    [ClassData(typeof(ModeratorChangeRoleClassData))]
    public async Task Handle_ShouldNotThrow_WhenModeratorChangeRole(string currentRole, string newRole)
    {
        var user = new IdentityUserDto(
            Guid.Empty,
            string.Empty,
            string.Empty,
            string.Empty);

        var currentUser = new ModeratorUser(user.Id);

        _authService
            .Setup(x => x.GetUserRoleAsync(user.TelegramUsername, default))
            .ReturnsAsync(currentRole);

        var command = new ChangeRole.Command(user.TelegramUsername, newRole);
        var handler = new ChangeRoleCommandHandler(_authService.Object, currentUser);

        Func<Task<ChangeRole.Response>> action = async () => await handler.Handle(command, default);

        await action.Should().NotThrowAsync();
    }

    [Theory]
    [ClassData(typeof(AdminChangeRoleClassData))]
    public async Task Handle_ShouldThrow_WhenModeratorPromoteToAdminOrManageAdmin(string currentRole, string newRole)
    {
        var user = new IdentityUserDto(
            Guid.Empty,
            string.Empty,
            string.Empty,
            string.Empty);

        var currentUser = new ModeratorUser(user.Id);

        _authService
            .Setup(x => x.GetUserRoleAsync(user.TelegramUsername, default))
            .ReturnsAsync(currentRole);

        var command = new ChangeRole.Command(user.TelegramUsername, newRole);
        var handler = new ChangeRoleCommandHandler(_authService.Object, currentUser);

        Func<Task<ChangeRole.Response>> action = async () => await handler.Handle(command, default);

        await action.Should().ThrowAsync<IdentityException>();
    }
}