﻿using FluentAssertions;
using VyazmaTech.Prachka.Application.Contracts.Core.Orders.Commands;
using VyazmaTech.Prachka.Application.Handlers.Core.Order.Commands.MarkOrderAsPaid;
using VyazmaTech.Prachka.Application.Handlers.Tests.Fixtures;
using VyazmaTech.Prachka.Domain.Common.Exceptions;
using VyazmaTech.Prachka.Domain.Core.Orders;
using VyazmaTech.Prachka.Tests.Tools.FluentBuilders;

namespace VyazmaTech.Prachka.Application.Handlers.Tests.Order.Commands;

public class MarkAsPaidTest : TestBase
{
    private static readonly VerifySettings Settings;
    private readonly MarkOrderAsPaidCommandHandler _handler;

    static MarkAsPaidTest()
    {
        Settings = new VerifySettings();
        Settings.UseDirectory("Contracts");
    }

    public MarkAsPaidTest(CoreDatabaseFixture fixture) : base(fixture)
    {
        _handler = new MarkOrderAsPaidCommandHandler(fixture.PersistenceContext);
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenOrderNotFound()
    {
        var orderId = Guid.NewGuid();
        var command = new MarkOrderAsPaid.Command(orderId, 0);

        Func<Task<MarkOrderAsPaid.Response>>
            action = async () => await _handler.Handle(command, CancellationToken.None);
        await action.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Verify_Contract()
    {
        // Arrange
        Domain.Core.Queues.Queue queue = Create.Queue
            .WithCapacity(2)
            .WithActivityBoundaries(TimeOnly.Parse("10:00"), TimeOnly.Parse("12:00"))
            .Build();

        Domain.Core.Users.User user = Create.User
            .WithFullname("Bobby Shmurda")
            .WithTelegramUsername("@bobster")
            .Build();

        var firstOrder = Create.Order
            .WithId(Guid.NewGuid())
            .WithUser(user)
            .WithQueue(queue)
            .WithStatus(OrderStatus.New)
            .Build();

        var secondOrder = Create.Order
            .WithId(Guid.NewGuid())
            .WithUser(user)
            .WithQueue(queue)
            .WithStatus(OrderStatus.New)
            .Build();

        PersistenceContext.Queues.InsertRange([queue]);
        PersistenceContext.Users.Insert(user);
        PersistenceContext.Orders.InsertRange([firstOrder, secondOrder]);
        await PersistenceContext.SaveChangesAsync(CancellationToken.None);

        var command = new MarkOrderAsPaid.Command(secondOrder.Id, 180);

        // Act
        MarkOrderAsPaid.Response response = await _handler.Handle(command, CancellationToken.None);

        // Assert
        await Verify(response, Settings);
    }
}