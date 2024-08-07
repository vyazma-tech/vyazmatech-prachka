#pragma warning disable IDE0008
using FluentAssertions;
using Newtonsoft.Json;
using VyazmaTech.Prachka.Domain.Core.Queues.Events;
using VyazmaTech.Prachka.Domain.Core.ValueObjects;
using VyazmaTech.Prachka.Domain.Kernel;
using Xunit;

namespace VyazmaTech.Prachka.Domain.Core.Tests.DomainEvents;

public sealed class QueueCreatedDomainEventTests
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
        var id = Guid.NewGuid();
        var activity = QueueActivityBoundaries.Create(TimeOnly.Parse("10:00"), TimeOnly.Parse("17:00"));
        var assignmentDate = AssignmentDate.Create(
            assignmentDate: DateTime.UtcNow.AddDays(1).AsDateOnly(),
            currentDate: DateTime.UtcNow.AsDateOnly());

        var @event = new QueueCreatedDomainEvent(id, activity, assignmentDate);
        string json = JsonConvert.SerializeObject(@event, Settings);

        // Act
        IDomainEvent? result = JsonConvert.DeserializeObject<IDomainEvent>(json, Settings);

        // Assert
        result.Should().NotBeNull();
        var domainEvent = result.Should().BeOfType<QueueCreatedDomainEvent>();

        domainEvent.Which.QueueId.Should().Be(id);
        domainEvent.Which.AssignmentDate.Should().Be(assignmentDate);
        domainEvent.Which.ActivityBoundaries.Should().Be(activity);
    }
}