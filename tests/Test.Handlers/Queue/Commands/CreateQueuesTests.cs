using Application.Handlers.Queue.Commands.CreateQueue;
using Domain.Kernel;
using FluentAssertions;
using Infrastructure.DataAccess.Repositories;
using Infrastructure.Tools;
using Microsoft.Extensions.Logging;
using Moq;
using Test.Handlers.Fixtures;
using Xunit;

namespace Test.Handlers.Queue.Commands;

public class CreateQueuesTests : TestBase
{
    private readonly CreateQueuesCommandHandler _handler;
    private readonly IDateTimeProvider _dateTimeProvider;

    public CreateQueuesTests(CoreDatabaseFixture database) : base(database)
    {
        ILogger<CreateQueuesCommandHandler> loggerMock = new Mock<ILogger<CreateQueuesCommandHandler>>().Object;
        var queueRepository = new QueueRepository(database.Context);
        _dateTimeProvider = new DateTimeProvider();

        _handler = new CreateQueuesCommandHandler(
            loggerMock,
            database.Context,
            queueRepository,
            _dateTimeProvider);
    }

    [Fact]
    public async Task CreateOneQueue_Should_ReturnCreatedQueue_WhenDataIsValid()
    {
        var command = new CreateQueuesCommand(new QueueModel[]
        {
            new (
                10,
                _dateTimeProvider.DateNow.AddDays(1),
                TimeOnly.FromDateTime(_dateTimeProvider.UtcNow.AddHours(2)),
                TimeOnly.FromDateTime(_dateTimeProvider.UtcNow.AddHours(4)))
        });

        CreateQueuesResponse response = await _handler.Handle(command, CancellationToken.None);

        response.Should().NotBeNull();
        response.Queues.Should().NotBeEmpty();
    }
}