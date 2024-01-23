using Application.DataAccess.Contracts.Repositories;
using Application.Handlers.Queue.Commands.IncreaseQueueCapacity;
using Domain.Common.Errors;
using Domain.Common.Result;
using Domain.Core.Queue;
using Domain.Kernel;
using FluentAssertions;
using Infrastructure.DataAccess.Contexts;
using Infrastructure.DataAccess.Models;
using Moq;
using Xunit;

namespace Test.Handlers.Queue.Commands;

public class IncreaseCapacityTest
{
    private readonly IncreaseQueueCapacityCommandHandler _handler;
    private readonly Mock<IQueueRepository> _queueRepository;

    public IncreaseCapacityTest()
    {
        var dateTimeProvider = new Mock<IDateTimeProvider>();
        var queues = new Mock<IQueueRepository>();
        IUserRepository users = Mock.Of<IUserRepository>();
        IOrderRepository orders = Mock.Of<IOrderRepository>();
        IOrderSubscriptionRepository orderSubscriptions = Mock.Of<IOrderSubscriptionRepository>();
        IQueueSubscriptionRepository queueSubscriptions = Mock.Of<IQueueSubscriptionRepository>();

        _queueRepository = queues;
        _handler = new IncreaseQueueCapacityCommandHandler(
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
        var command = new IncreaseQueueCapacity.Command(queueId, 2);

        _queueRepository.Setup(x =>
                x.FindByAsync(
                It.IsAny<Specification<QueueModel>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(
                new Result<QueueEntity>(
                    DomainErrors.Entity.NotFoundFor<QueueEntity>($"QueueId = {queueId}")));

        Result<IncreaseQueueCapacity.Response> response = await _handler.Handle(command, CancellationToken.None);


        response.IsFaulted.Should().BeTrue();
        response.Error.Should().Be(DomainErrors.Entity.NotFoundFor<QueueEntity>($"QueueId = {queueId}"));
    }
}