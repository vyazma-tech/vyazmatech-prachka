using FluentAssertions;
using VyazmaTech.Prachka.Domain.Core.Orders;
using VyazmaTech.Prachka.Domain.Kernel;
using VyazmaTech.Prachka.Infrastructure.DataAccess.Repositories;
using VyazmaTech.Prachka.Infrastructure.DataAccess.Tests.Fixtures;
using VyazmaTech.Prachka.Tests.Tools.FluentBuilders;

namespace VyazmaTech.Prachka.Infrastructure.DataAccess.Tests;

public sealed class ReportRepositoryTests : TestBase
{
    private readonly ReportRepository _repository;

    public ReportRepositoryTests(InfrastructureDatabaseFixture fixture) : base(fixture)
    {
        _repository = new ReportRepository(fixture.Context);
    }

    [Fact]
    public async Task GetReports_ShouldGroupByMonth_WhenSeveralMonthsCovered()
    {
        // Arrange
        var user = Create.User
            .WithFullname("Bobby Shmurda")
            .WithTelegramUsername("@bobster")
            .Build();

        var queue = Create.Queue
            .WithCapacity(100)
            .WithAssignmentDate(DateTime.UtcNow.AsDateOnly())
            .WithActivityBoundaries(TimeOnly.MinValue, TimeOnly.MaxValue)
            .Build();

        var firstMonthOrders = Enumerable.Range(0, 5)
            .Select(_ =>
                Create.Order
                    .WithUser(user)
                    .WithStatus(OrderStatus.Paid)
                    .WithCreationDate(DateTime.UtcNow.AsDateOnly())
                    .Build());

        var secondMonthOrders = Enumerable.Range(0, 5)
            .Select(_ =>
                Create.Order
                    .WithUser(user)
                    .WithStatus(OrderStatus.Prolonged)
                    .WithCreationDate(DateTime.UtcNow.AddMonths(1).AsDateOnly())
                    .Build());

        queue.BulkInsert([..firstMonthOrders, ..secondMonthOrders]);
        Context.Queues.Add(queue);
        await Context.SaveChangesAsync();

        // Act
        var reports = await _repository
            .GetReports(
                DateTime.UtcNow.AsDateOnly(),
                DateTime.UtcNow.AddMonths(1).AsDateOnly());

        // Assert
        reports.Count.Should().Be(2);
    }

    [Fact]
    public async Task GetReports_ShouldSumOrders_WhenSeveralOrdersFromUser()
    {
        // Arrange
        var user = Create.User
            .WithFullname("Bobby Shmurda")
            .WithTelegramUsername("@bobster")
            .Build();

        var queue = Create.Queue
            .WithCapacity(100)
            .WithAssignmentDate(DateTime.UtcNow.AsDateOnly())
            .WithActivityBoundaries(TimeOnly.MinValue, TimeOnly.MaxValue)
            .Build();

        var firstMonthOrders = Enumerable.Range(0, 5)
            .Select(_ =>
                Create.Order
                    .WithUser(user)
                    .WithStatus(OrderStatus.Paid)
                    .WithCreationDate(DateTime.UtcNow.AsDateOnly())
                    .WithPrice(180.0)
                    .Build());

        queue.BulkInsert([..firstMonthOrders]);
        Context.Queues.Add(queue);
        await Context.SaveChangesAsync();

        // Act
        var reports = await _repository
            .GetReports(
                DateTime.UtcNow.AsDateOnly(),
                DateTime.UtcNow.AddMonths(1).AsDateOnly());

        // Assert
        reports.Should()
            .ContainSingle()
            .Which.LineItems.Should()
            .ContainSingle()
            .Which.OrderPrice.Should()
            .Be(900);
    }
}