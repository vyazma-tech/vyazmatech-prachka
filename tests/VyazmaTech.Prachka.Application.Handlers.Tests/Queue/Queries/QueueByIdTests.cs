using VyazmaTech.Prachka.Application.Contracts.Core.Queues.Queries;
using VyazmaTech.Prachka.Application.Handlers.Core.Queue.Queries;
using VyazmaTech.Prachka.Application.Handlers.Tests.Fixtures;
using VyazmaTech.Prachka.Domain.Kernel;
using VyazmaTech.Prachka.Tests.Tools.FluentBuilders;

namespace VyazmaTech.Prachka.Application.Handlers.Tests.Queue.Queries;

public sealed class QueueByIdTests : TestBase
{
    private static readonly VerifySettings Settings;
    private readonly QueueByIdQueryHandler _handler;

    static QueueByIdTests()
    {
        Settings = new VerifySettings();
        Settings.UseDirectory("Contracts");
        Settings.DontIgnoreEmptyCollections();
    }

    public QueueByIdTests(CoreDatabaseFixture fixture) : base(fixture)
    {
        _handler = new QueueByIdQueryHandler(fixture.PersistenceContext);
    }

    [Fact]
    public async Task Verify_Contract()
    {
        // Arrange
        var queue = Create.Queue
            .WithCapacity(1)
            .WithAssignmentDate(DateTime.UtcNow.AsDateOnly())
            .WithActivityBoundaries(TimeOnly.MinValue, TimeOnly.MaxValue)
            .Build();

        PersistenceContext.Queues.InsertRange([queue]);
        await PersistenceContext.SaveChangesAsync(CancellationToken.None);

        // Act
        var query = new QueueById.Query(queue.Id);
        var response = await _handler.Handle(query, CancellationToken.None);

        // Assert
        await Verify(response, Settings);
    }
}