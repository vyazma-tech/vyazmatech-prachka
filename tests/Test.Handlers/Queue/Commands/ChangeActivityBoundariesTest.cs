using Application.Core.Common;
using Application.Handlers.Queue.Commands.ChangeQueueActivityBoundaries;
using Application.Handlers.Queue.Commands.IncreaseQueueCapacity;
using Application.Handlers.Queue.Queries;
using Domain.Common.Errors;
using Domain.Common.Result;
using Domain.Core.Queue;
using Domain.Core.ValueObjects;
using Domain.Kernel;
using FluentAssertions;
using Infrastructure.DataAccess.Contexts;
using Infrastructure.DataAccess.Contracts;
using Infrastructure.DataAccess.Models;
using Infrastructure.DataAccess.Repositories;
using Infrastructure.Tools;
using Moq;
using Test.Handlers.Fixtures;
using Xunit;

namespace Test.Handlers.Queue.Commands;

public class ChangeActivityBoundariesTest
{
    private readonly ChangeQueueActivityBoundariesCommandHandler _handler;
    private readonly Mock<IQueueRepository> _queueRepository;

    public ChangeActivityBoundariesTest()
    {
        var dateTimeProvider = new Mock<IDateTimeProvider>();
        var queues = new Mock<IQueueRepository>();
        IUserRepository users = Mock.Of<IUserRepository>();
        IOrderRepository orders = Mock.Of<IOrderRepository>();
        IOrderSubscriptionRepository orderSubscriptions = Mock.Of<IOrderSubscriptionRepository>();
        IQueueSubscriptionRepository queueSubscriptions = Mock.Of<IQueueSubscriptionRepository>();

        _queueRepository = queues;
        _handler = new ChangeQueueActivityBoundariesCommandHandler(
            dateTimeProvider.Object,
            new PersistenceContext(
                queues.Object,
                orders,
                users,
                orderSubscriptions,
                queueSubscriptions,
                null!));
    }


    [Fact]
    public async Task Handle_ShouldReturnFailureResult_WhenQueueIsNotFound()
    {
        var queueId = Guid.NewGuid();
        var command = new ChangeQueueActivityBoundaries.Command(queueId, TimeOnly.MinValue, TimeOnly.MaxValue);

        _queueRepository.Setup(x =>
                x.FindByAsync(
                    It.IsAny<Specification<QueueModel>>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                new Result<QueueEntity>(
                    DomainErrors.Entity.NotFoundFor<QueueEntity>($"QueueId = {queueId}")));

        Result<ChangeQueueActivityBoundaries.Response> response = await _handler.Handle(command, CancellationToken.None);


        response.IsFaulted.Should().BeTrue();
        response.Error.Should().Be(DomainErrors.Entity.NotFoundFor<QueueEntity>($"QueueId = {queueId}"));
    }
}