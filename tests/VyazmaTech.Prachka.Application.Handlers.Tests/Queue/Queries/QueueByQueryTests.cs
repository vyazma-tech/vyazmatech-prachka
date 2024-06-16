using Microsoft.Extensions.Options;
using Moq;
using VyazmaTech.Prachka.Application.Abstractions.Configuration;
using VyazmaTech.Prachka.Application.Contracts.Queues.Queries;
using VyazmaTech.Prachka.Application.Handlers.Queue.Queries;
using VyazmaTech.Prachka.Application.Handlers.Tests.Fixtures;
using VyazmaTech.Prachka.Domain.Kernel;
using VyazmaTech.Prachka.Tests.Tools.FluentBuilders;

namespace VyazmaTech.Prachka.Application.Handlers.Tests.Queue.Queries;

public sealed class QueueByQueryTests : TestBase
{
    private static readonly VerifySettings Settings;
    private readonly QueueByQueryQueryHandler _handler;

    static QueueByQueryTests()
    {
        Settings = new VerifySettings();
        Settings.UseDirectory("Contracts");
        Settings.DontIgnoreEmptyCollections();
    }

    public QueueByQueryTests(CoreDatabaseFixture fixture) : base(fixture)
    {
        var timeProvider = new Mock<IDateTimeProvider>();
        var pagination = new Mock<IOptions<PaginationConfiguration>>();

        pagination
            .Setup(x => x.Value)
            .Returns(new PaginationConfiguration { RecordsPerPage = 10 });

        _handler = new QueueByQueryQueryHandler(fixture.PersistenceContext, pagination.Object, timeProvider.Object);
    }

    [Fact]
    public async Task Verify_Contract()
    {
        // Arrange
        var assignmentDate = DateTime.UtcNow.AsDateOnly();
        var queue = Create.Queue
            .WithCapacity(1)
            .WithAssignmentDate(assignmentDate)
            .WithActivityBoundaries(TimeOnly.MinValue, TimeOnly.MaxValue)
            .Build();

        PersistenceContext.Queues.InsertRange([queue]);
        await PersistenceContext.SaveChangesAsync(CancellationToken.None);

        // Act
        var query = new QueueByQuery.Query(assignmentDate, 0);
        var response = await _handler.Handle(query, CancellationToken.None);

        // Assert
        await Verify(response, Settings);
    }
}