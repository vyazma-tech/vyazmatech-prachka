#pragma warning disable CA1506
using FluentAssertions;
using Moq;
using VyazmaTech.Prachka.Application.Abstractions.Identity;
using VyazmaTech.Prachka.Application.Contracts.Core.Orders.Queries;
using VyazmaTech.Prachka.Application.Core.Errors;
using VyazmaTech.Prachka.Application.Core.Users;
using VyazmaTech.Prachka.Application.Handlers.Core.Order.Queries;
using VyazmaTech.Prachka.Application.Handlers.Tests.Fixtures;
using VyazmaTech.Prachka.Domain.Common.Exceptions;
using VyazmaTech.Prachka.Domain.Core.Orders;
using VyazmaTech.Prachka.Domain.Core.Queues;
using VyazmaTech.Prachka.Domain.Kernel;
using VyazmaTech.Prachka.Tests.Tools.FluentBuilders;

namespace VyazmaTech.Prachka.Application.Handlers.Tests.Order.Queries;

public sealed class MyOrdersTests : TestBase
{
    private static readonly VerifySettings Settings;
    private readonly MyOrdersQueryHandler _handler;
    private readonly Mock<ICurrentUser> _currentUser;

    static MyOrdersTests()
    {
        Settings = new VerifySettings();
        Settings.UseDirectory("Contracts");
    }

    public MyOrdersTests(CoreDatabaseFixture fixture) : base(fixture)
    {
        _currentUser = new Mock<ICurrentUser>();
        _handler = new MyOrdersQueryHandler(_currentUser.Object, fixture.PersistenceContext);
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenUserIsNotAuthenticated()
    {
        // Arrange
        var user = new AnonymousUser();
        _currentUser.Setup(x => x.Id).Returns(user.Id);

        // Act
        var query = default(MyOrders.Query);
        var action = async () => await _handler.Handle(query, default);

        // Assert
        var exception = await action.Should().ThrowAsync<IdentityException>();
        exception.Which.Error.Should().Be(ApplicationErrors.MyOrders.AnonymousUserCantSeeTheirOrders);
    }

    [Fact]
    public async Task Handle_Should_GroupByQueueDate_And_SortInDescendingOrder()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _currentUser.Setup(x => x.Id).Returns(userId);

        var user = Create.User
            .WithFullname("Bobby Shmurda")
            .WithTelegramUsername("@bobster")
            .WithId(userId)
            .Build();

        var queues = Enumerable.Range(1, 2)
            .Select(x =>
                Create.Queue
                    .WithCapacity(10)
                    .WithCreationDate(DateTime.UtcNow.AsDateOnly().AddDays(x))
                    .WithAssignmentDate(DateTime.UtcNow.AsDateOnly().AddDays(x))
                    .WithActivityBoundaries(TimeOnly.MinValue, TimeOnly.MaxValue)
                    .WithState(QueueState.Active)
                    .Build())
            .ToList();

        var firstDayOrders = Enumerable.Range(1, 10)
            .Select(x =>
                Create.Order
                    .WithUser(user)
                    .WithCreationDate(DateTime.UtcNow.AsDateOnly().AddDays(x))
                    .WithModification(DateTime.UtcNow)
                    .Build());

        var secondDayOrders = Enumerable.Range(1, 10)
            .Select(x =>
                Create.Order
                    .WithUser(user)
                    .WithCreationDate(DateTime.UtcNow.AsDateOnly().AddDays(x))
                    .WithModification(DateTime.UtcNow)
                    .Build());

        queues.First().BulkInsert([..firstDayOrders]);
        queues.Last().BulkInsert([..secondDayOrders]);
        PersistenceContext.Queues.InsertRange(queues);
        await PersistenceContext.SaveChangesAsync(default);

        // Act
        var query = default(MyOrders.Query);
        var response = await _handler.Handle(query, default);

        // Assert
        response.History.Should()
            .HaveCount(2)
            .And.Subject.Should()
            .AllSatisfy(queue =>
                queue.Orders.Should()
                    .BeInDescendingOrder(x =>
                        x.CreationDate));
    }

    [Fact]
    public async Task Verify_Contract()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _currentUser.Setup(x => x.Id).Returns(userId);

        var user = Create.User
            .WithId(userId)
            .WithFullname("Bobby Shmurda")
            .WithTelegramUsername("@bobster")
            .Build();

        var queue = Create.Queue
            .WithCapacity(20)
            .WithActivityBoundaries(TimeOnly.Parse("10:00"), TimeOnly.Parse("17:00"))
            .Build();

        var todayOrders = Enumerable.Range(0, 10)
            .Select(_ =>
                Create.Order
                    .WithUser(user)
                    .WithStatus(OrderStatus.New)
                    .WithCreationDate(DateTime.Today.AsDateOnly())
                    .Build());

        var yesterdayOrders = Enumerable.Range(0, 10)
            .Select(_ =>
                Create.Order
                    .WithUser(user)
                    .WithStatus(OrderStatus.New)
                    .WithCreationDate(DateTime.Today.AsDateOnly().AddDays(-1))
                    .Build());

        queue.BulkInsert([..todayOrders, ..yesterdayOrders]);

        PersistenceContext.Users.Insert(user);
        PersistenceContext.Queues.InsertRange([queue]);

        await PersistenceContext.SaveChangesAsync(default);

        // Act
        var query = default(MyOrders.Query);
        var response = await _handler.Handle(query, default);

        // Assert
        await Verify(response, Settings);
    }
}