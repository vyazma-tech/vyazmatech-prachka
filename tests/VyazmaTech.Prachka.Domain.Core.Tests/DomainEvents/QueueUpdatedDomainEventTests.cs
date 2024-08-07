#pragma warning disable IDE0008
using FluentAssertions;
using Newtonsoft.Json;
using VyazmaTech.Prachka.Domain.Core.Queues;
using VyazmaTech.Prachka.Domain.Core.Queues.Events;
using VyazmaTech.Prachka.Domain.Kernel;
using VyazmaTech.Prachka.Tests.Tools.FluentBuilders;
using Xunit;

namespace VyazmaTech.Prachka.Domain.Core.Tests.DomainEvents;

public sealed class QueueUpdatedDomainEventTests
{
    private static readonly JsonSerializerSettings Settings = new()
    {
        TypeNameHandling = TypeNameHandling.All,
        ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
    };

    [Fact]
    public void DeserializeObject_Should_DeserializeAllProperties()
    {
        // Arrange
        var queue = Create.Queue
            .WithId(Guid.NewGuid())
            .WithCapacity(10)
            .WithAssignmentDate(DateTime.UtcNow.AsDateOnly())
            .WithActivityBoundaries(TimeOnly.Parse("10:00"), TimeOnly.Parse("17:00"))
            .WithState(QueueState.Active)
            .Build();

        var @event = QueueUpdatedDomainEvent.From(queue);
        string json = JsonConvert.SerializeObject(@event, Settings);

        // Act
        IDomainEvent? result = JsonConvert.DeserializeObject<IDomainEvent>(json, Settings);

        // Assert
        result.Should().NotBeNull();
        var domainEvent = result.Should().BeOfType<QueueUpdatedDomainEvent>();

        domainEvent.Which.Id.Should().Be(queue.Id);
        domainEvent.Which.Capacity.Should().Be(queue.Capacity);
        domainEvent.Which.AssignmentDate.Should().Be(queue.AssignmentDate);
        domainEvent.Which.ActivityBoundaries.Should().Be(queue.ActivityBoundaries);
        domainEvent.Which.State.Should().Be(queue.State);
    }
}