using FluentAssertions;
using Moq;
using VyazmaTech.Prachka.Application.Abstractions.Identity;
using VyazmaTech.Prachka.Application.Contracts.Core.Queues.Commands;
using VyazmaTech.Prachka.Application.Core.Errors;
using VyazmaTech.Prachka.Application.Handlers.Core.Queue.Commands.BulkRemoveOrders;
using VyazmaTech.Prachka.Application.Handlers.Tests.Fixtures;
using VyazmaTech.Prachka.Domain.Common.Errors;
using VyazmaTech.Prachka.Domain.Common.Exceptions;
using VyazmaTech.Prachka.Tests.Tools.FluentBuilders;

namespace VyazmaTech.Prachka.Application.Handlers.Tests.Queue.Commands;

public sealed class BulkRemoveTests : TestBase
{
    private readonly BulkRemoveOrdersCommandHandler _handler;
    private readonly Mock<ICurrentUser> _currentUser;

    public BulkRemoveTests(CoreDatabaseFixture fixture) : base(fixture)
    {
        _currentUser = new Mock<ICurrentUser>();
        _handler = new BulkRemoveOrdersCommandHandler(
            fixture.PersistenceContext,
            _currentUser.Object);
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenCurrentUserNotAuthenticated()
    {
        // Arrange
        _currentUser.Setup(x => x.Id).Returns((Guid?)null);

        // Act
        var command = new BulkRemoveOrders.Command(default, 1);
        Func<Task<BulkRemoveOrders.Response>> action = async () =>
            await _handler.Handle(command, CancellationToken.None);

        // Assert
        var exception = await action.Should().ThrowAsync<IdentityException>();
        exception.Which.Error.Should()
            .Be(ApplicationErrors.BulkInsertOrders.AnonymousUserCantEnter);
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenUserNotFound()
    {
        // Arrange
        _currentUser.Setup(x => x.Id).Returns(Guid.NewGuid());

        // Act
        var command = new BulkRemoveOrders.Command(default, 1);
        Func<Task<BulkRemoveOrders.Response>> action = async () =>
            await _handler.Handle(command, CancellationToken.None);

        // Assert
        var exception = await action.Should().ThrowAsync<NotFoundException>();
        exception.Which.Error.Should()
            .Be(DomainErrors.User.NotFound);
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenQueueNotFound()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _currentUser.Setup(x => x.Id).Returns(userId);

        var user = Create.User
            .WithId(userId)
            .WithFullname("Bobby Shmurda")
            .WithTelegramUsername("@bobster")
            .Build();

        PersistenceContext.Users.Insert(user);
        await PersistenceContext.SaveChangesAsync(CancellationToken.None);

        // Act
        var command = new BulkRemoveOrders.Command(default, 0);
        Func<Task<BulkRemoveOrders.Response>> action = async () =>
            await _handler.Handle(command, CancellationToken.None);

        // Assert
        var exception = await action.Should().ThrowAsync<NotFoundException>();
        exception.Which.Error.Should()
            .Be(DomainErrors.Queue.NotFound);
    }
}